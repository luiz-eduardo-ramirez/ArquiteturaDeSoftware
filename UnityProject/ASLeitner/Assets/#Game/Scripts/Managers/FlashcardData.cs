using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner.DataStructs
{
    public enum LearningStages { Ignorant, Superficial, Acquired }
    [Serializable]
    public class FlashcardData
    {
        [SerializeField] private string m_cardFront;
        [SerializeField] private string m_cardBack;
        [SerializeField] private LearningStages m_learningStage;

        public FlashcardData(string _cardFront, string _cardBack, LearningStages _learningStage)
        {
            if (_cardFront.Length > 120) throw new Exception("Termo com caracteres em excesso");
            if (_cardBack.Length > 430) throw new Exception("Definicao com caracteres em excesso");
            m_cardFront = _cardFront;
            m_cardBack = _cardBack;
            m_learningStage = _learningStage;
        }

        public string CardFront{ get => m_cardFront; } //max 120 chars 
        public string CardBack{ get => m_cardBack; } //max 430 chars
        public LearningStages LearningStage{ get => m_learningStage; }
    }
}
