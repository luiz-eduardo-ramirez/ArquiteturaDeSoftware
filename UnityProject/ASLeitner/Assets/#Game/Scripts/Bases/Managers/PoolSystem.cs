using Base.Extensions;
using Base.Extensions.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Managers
{
    public class PoolSystem : MonoSingleton<PoolSystem>
    {
        [Serializable] public struct PoolingDataStruct
        {
            public MonoInstance ObjToPool;

            public int PoolAmount;
        }

        [Header("PoolingSystem data")]
        [SerializeField] private Transform m_objsSpawnPoint;
        [SerializeField] private PoolingDataStruct[] m_poolingData;

        private Dictionary<int, Queue<MonoInstance>> m_pooledObjs;
        private Transform[] m_objsParents;

        protected override void Awake()
        {
            base.Awake();
            m_pooledObjs = new Dictionary<int, Queue<MonoInstance>>();

            for (int i = 0; i < m_poolingData.Length; i++) m_pooledObjs.Add(m_poolingData[i].ObjToPool.GetInstanceID(), new Queue<MonoInstance>());
            
            InstantiateAllPoolable();
        }

        private void InstantiateAllPoolable()
        {
            m_objsParents = new Transform[m_poolingData.Length];
            for (int i = 0; i < m_poolingData.Length; i++)
            {
                Transform newParent;

                newParent = Instantiate(new GameObject(), transform).transform;
                newParent.name = m_poolingData[i].ObjToPool.name + " Pool Parent";
                newParent.transform.position = m_objsSpawnPoint.position;
                m_objsParents[i] = newParent;

                for (int index = 0; index < m_poolingData[i].PoolAmount; index++)
                {
                    InstantiatePoolable(m_poolingData[i].ObjToPool, newParent);
                }
            }
        }
        private void InstantiatePoolable(MonoInstance _objPrefabToPool, Transform _parent)
        {
            MonoInstance newObj = Instantiate(_objPrefabToPool, _parent.transform.position, _objPrefabToPool.transform.rotation, _parent);

            newObj.gameObject.SetActive(false);
            m_pooledObjs[_objPrefabToPool.GetInstanceID()].Enqueue(newObj);
        }

        public static MonoInstance GetObj(MonoInstance _prefab, Transform _newParent = null)
        {
            MonoInstance objToReturn;
            if (Instance.m_pooledObjs[_prefab.GetInstanceID()].Count > 0)
                objToReturn = Instance.m_pooledObjs[_prefab.GetInstanceID()].Dequeue();
            else objToReturn = null;

            if (objToReturn != null)
            {
                if (_newParent != null) objToReturn.transform.parent = _newParent;
                objToReturn.gameObject.SetActive(true);
                return objToReturn;
            }

            Debug.LogWarning("No pooled " + _prefab.name + " found, returning null!!!");
            return null;
        }

        public static void ReturnObjToPool(MonoInstance _objPrefab, MonoInstance _returningObj)
        {
            if (!Instance.m_pooledObjs[_objPrefab.GetInstanceID()].Contains(_returningObj))
            {
                _returningObj.transform.position = Instance.m_objsSpawnPoint.position;
                _returningObj.gameObject.SetActive(false);
                Instance.m_pooledObjs[_objPrefab.GetInstanceID()].Enqueue(_returningObj);
            }
            else
            {
                Debug.LogWarning("Obj already pooled !!!");
            }
        }
    }
}
