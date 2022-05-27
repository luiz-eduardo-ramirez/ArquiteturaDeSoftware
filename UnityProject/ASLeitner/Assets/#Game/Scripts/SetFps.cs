using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    public class SetFps : MonoBehaviour
    {
        [SerializeField]
        private int m_targetFps;
        private void Awake()
        {
            Application.targetFrameRate = m_targetFps;
        }
    }
}
