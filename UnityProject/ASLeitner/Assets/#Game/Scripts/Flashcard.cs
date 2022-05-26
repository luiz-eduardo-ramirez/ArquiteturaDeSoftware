using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ASLeitner
{
    public class Flashcard : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro m_termo;
        [SerializeField]
        private TextMeshPro m_definicao;
        private void Update()
        {
            LookForward();
        }
        private void LookForward()
        {
            transform.forward = Vector3.back;
        }
        public void SetFlashCard(FlashcardData _flashcardData)
        {
            m_termo.text = _flashcardData.CardFront;
            m_definicao.text = _flashcardData.CardBack;
        }
    }
}
