using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.Extensions.Utilites
{
    public interface ICollisionEnterDelegated { void OnCollisionEnterDelegated(Collision other); }
    
    [ExecuteInEditMode]
    public class DelegateCollisionCaller : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Update()
        {
            if (!(m_OnCollisionEnterMono is ICollisionEnterDelegated))
            {
                m_OnCollisionEnterMono = null;
                Debug.LogError("Objc isnt a ICollisionEnterDelegated");
            }
        }
#endif

        [SerializeField] private MonoBehaviour m_OnCollisionEnterMono;

        private ICollisionEnterDelegated m_OnCollisionEnter;

        private void Awake()
        {
            if ((m_OnCollisionEnterMono is ICollisionEnterDelegated))
            {
                m_OnCollisionEnter = (ICollisionEnterDelegated)m_OnCollisionEnterMono;
            }
            else
            {
                m_OnCollisionEnterMono = null;
                Debug.LogError("Objc isnt a ICollisionEnterDelegated");
            }
        }

        private void OnCollisionEnter(Collision other)
        {
           if(m_OnCollisionEnter != null) m_OnCollisionEnter.OnCollisionEnterDelegated(other);
        }
    }
}
