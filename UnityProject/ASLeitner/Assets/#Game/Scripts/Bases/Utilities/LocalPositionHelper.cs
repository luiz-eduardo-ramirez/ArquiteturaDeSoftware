using Base.Extensions;
using Base.Extensions.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Utilites
{
    [ExecuteInEditMode]
    public class LocalPositionHelper : MonoInstance
    {
#if UNITY_EDITOR

        [Header("Local Move")]
        [SerializeField] private Vector3 m_localMoveParam;
        [SerializeField] private bool m_executeLocalMove;

        [Header("Store Position Helper")]
        [SerializeField, ReadOnly] private Vector3 m_storedPos;
        [SerializeField] private bool m_storeCurrentPos;
        [SerializeField] private bool m_moveToStoredPos;
        [SerializeField, ReadOnly] private float m_distanceFromStoredPos;
        // Update is called once per frame
        void Update()
        {
            CheckForExecuteOrder();
        }
        private void CheckForExecuteOrder()
        {
            m_distanceFromStoredPos = Vector3.Distance(transform.position, m_storedPos);
            if (m_executeLocalMove)
            {
                transform.Translate(m_localMoveParam);
                m_executeLocalMove = false;
            }
            else if (m_storeCurrentPos)
            {
                m_storedPos = transform.position;
                m_storeCurrentPos = false;
            }
            else if (m_moveToStoredPos)
            {
                transform.position = m_storedPos;
                m_moveToStoredPos = false;
            }
        }
#endif
    }
}
