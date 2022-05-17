using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Extensions.Utilites
{
    public class ActionTriggerCaller : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_onTriggerEnterEvents;
        private void OnTriggerEnter(Collider other)
        {
            if (m_onTriggerEnterEvents != null) m_onTriggerEnterEvents.Invoke();
        }
        public void InvokeEvent()
        {
            if (m_onTriggerEnterEvents != null) m_onTriggerEnterEvents.Invoke();
        }
    }
}
