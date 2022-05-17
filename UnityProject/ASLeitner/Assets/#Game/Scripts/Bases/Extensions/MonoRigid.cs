using Base.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Base.Extensions
{

    public class MonoRigid : MonoInstance
    {
        [SerializeField] private Rigidbody m_rigidbody;

        public new Rigidbody rigidbody
        {
            get
            {
                if (m_rigidbody == null)
                {
                    m_rigidbody = GetComponent<Rigidbody>();
                }
                return m_rigidbody;
            }
        }
    }
}
