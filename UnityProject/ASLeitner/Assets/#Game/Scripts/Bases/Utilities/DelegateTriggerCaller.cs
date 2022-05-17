using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Base.Extensions.Utilites
{
    public interface ITriggerEnterDelegated { void OnTriggerEnterDelegated(Collider other); }

    public class DelegateTriggerCaller : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private MonoBehaviour m_OnTriggerEnterMono;

        private ITriggerEnterDelegated m_OnTriggerEnter;

        private void Awake()
        {
            if ((m_OnTriggerEnterMono is ITriggerEnterDelegated))
            {
                m_OnTriggerEnter = (ITriggerEnterDelegated)m_OnTriggerEnterMono;
            }
            else
            {
                m_OnTriggerEnterMono = null;
                Debug.LogError("Objc isnt a ICollisionEnterDelegated");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (m_OnTriggerEnter != null) m_OnTriggerEnter.OnTriggerEnterDelegated(other);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (!(m_OnTriggerEnterMono is ITriggerEnterDelegated))
            {
                m_OnTriggerEnterMono = null;
                Debug.LogError("Objc isnt a ITriggerEnterDelegated");
            }
        }
    }
}
