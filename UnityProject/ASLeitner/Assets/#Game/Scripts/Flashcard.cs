using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System;
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
        [SerializeField]
        private Color m_ignoranceColor;
        [SerializeField]
        private Color m_superficialColor;
        [SerializeField]
        private Color m_acquiredColor;
        [SerializeField]
        private Renderer m_rendererBack;
        [SerializeField]
        private Renderer m_rendererFront;
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

            switch (_flashcardData.LearningStage)
            {
                case LearningStages.Ignorant:
                    m_rendererBack.material.SetColor("_EmissionColor", m_ignoranceColor);
                    m_rendererFront.material.SetColor("_EmissionColor", m_ignoranceColor);
                    break;
                case LearningStages.Superficial:
                    m_rendererBack.material.SetColor("_EmissionColor", m_superficialColor);
                    m_rendererFront.material.SetColor("_EmissionColor", m_superficialColor);
                    break;
                case LearningStages.Acquired:
                    m_rendererBack.material.SetColor("_EmissionColor", m_acquiredColor);
                    m_rendererFront.material.SetColor("_EmissionColor", m_acquiredColor);
                    break;
                default:
                    throw new Exception("Estagio de aprendizado inexistente");
            }
        }
        public void SetTermVisibility(bool _setValue) 
        {
            m_termo.enabled = _setValue;
        }
        public void SetDefinitionVisibility(bool _setValue)
        {
            m_definicao.enabled = _setValue;
        }
    }
}
