using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System;

namespace ASLeitner
{
    public class LearningCtrl : MonoBehaviour
    {
        public static LearningStages CurrentLearningStage { private get; set; }
        public static PlayerDataManager.LearningSets LearningSets { private get; set; }

        private List<FlashcardData> m_learningStageFlashcards;

        public List<FlashcardData> LearningStageFlashcards { get => m_learningStageFlashcards; }
        private void Awake()
        {
            BuildLearningDeck();
        }
        private void BuildLearningDeck()
        {
            m_learningStageFlashcards = new List<FlashcardData>();

            switch (CurrentLearningStage)
            {
                case LearningStages.Ignorant:
                    m_learningStageFlashcards.AddRange(LearningSets.Ignorance);
                    break;
                case LearningStages.Superficial:
                    m_learningStageFlashcards.AddRange(LearningSets.Ignorance);
                    m_learningStageFlashcards.AddRange(LearningSets.Superficial);
                    break;
                case LearningStages.Acquired:
                    m_learningStageFlashcards.AddRange(LearningSets.Ignorance);
                    m_learningStageFlashcards.AddRange(LearningSets.Superficial);
                    m_learningStageFlashcards.AddRange(LearningSets.Acquired);
                    break;
                default:
                    throw new Exception("Estagio de aprendizado inexistente");
            }
        }
        public void LearnFlashcard(FlashcardData _flashCard)
        {
            LearningStages newStage = _flashCard.LearningStage == LearningStages.Acquired ? LearningStages.Acquired : _flashCard.LearningStage + 1;
            FlashcardData newFlashcard = new FlashcardData(_flashCard.CardFront, _flashCard.CardBack, newStage);
            PlayerDataManager.Instance.SetFlashcard(_flashCard.CardFront, newFlashcard);
        }
        public void ForgetFlashcard(FlashcardData _flashCard)
        {
            LearningStages newStage = LearningStages.Ignorant;
            FlashcardData newFlashcard = new FlashcardData(_flashCard.CardFront, _flashCard.CardBack, newStage);
            PlayerDataManager.Instance.SetFlashcard(_flashCard.CardFront, newFlashcard);

        }
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(SceneRefs.MainMenu);
        }
        public void OnAllFlashcardsLearned()
        {
            PlayerDataManager.Instance.SaveFlashcards(() => SceneManager.LoadScene(SceneRefs.MainMenu));            
        }
    }
}
