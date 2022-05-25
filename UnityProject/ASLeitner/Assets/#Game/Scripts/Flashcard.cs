using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    public class Flashcard : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Vector3.back;
        }
    }
}
