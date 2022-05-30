using ASLeitner.DataStructs;
using ASLeitner.Managers;
using Base.Extensions.Utilites;
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
        public bool ShowingTerm { get; set; }
        public bool LookForward { get; set; }

        public FlashcardData FlashcardData { get; private set; }
        private void Start()
        {
            ShowingTerm = true;
        }
        private void Update()
        {
            if(LookForward) RotateForward();
        }
        private void RotateForward()
        {
            transform.forward = ShowingTerm ? Vector3.back : Vector3.forward;
        }
        public void SetFlashCard(FlashcardData _flashcardData)
        {
            m_termo.text = _flashcardData.CardFront;
            m_definicao.text = _flashcardData.CardBack;
            FlashcardData = _flashcardData;
        }
    }
}
