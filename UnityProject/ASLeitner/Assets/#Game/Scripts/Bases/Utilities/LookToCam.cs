using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Utilites
{
    public class LookToCam : MonoInstance
    {
        [SerializeField] private Transform m_mainCamTransform;
        private void Start()
        {
            if (m_mainCamTransform == null)
            {
                Debug.LogError("null reference on main cam transform");
                Destroy(gameObject);
            }
        }
        private void LateUpdate()
        {
            transform.LookAt(m_mainCamTransform);
        }
    }
}
