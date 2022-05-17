using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions
{
    public abstract class MonoInstance : MonoBehaviour
    {
        private Transform m_tr;
        private GameObject m_obj;
        /// <summary>
        /// Cache do transform da unity;
        /// </summary>
        public new Transform transform 
        {
            get 
            {
                if (m_tr == null) m_tr = base.transform;
                return m_tr;
            }
        }

        /// <summary>
        /// Cache do gameObject da unity;
        /// </summary>
        public new GameObject gameObject 
        {
            get 
            {
                if (m_obj == null) m_obj = base.gameObject;
                return m_obj;
            }
        }
    }
}
