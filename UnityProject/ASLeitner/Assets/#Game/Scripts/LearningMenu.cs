using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System;
using System.Linq;

namespace ASLeitner
{
    public class LearningMenu : MonoBehaviour
    {
        public static LearningStages CurrentLearningStage { private get; set; }
        public static PlayerDataManager.LearningSets LearningSets { private get; set; }

        [SerializeField] private MagicalRouletteCtrl m_rouletteCtrl;

        private List<FlashcardData> m_learningStageFlashcards;

        private void Awake()
        {
            BuildLearningDeck();
        }
        private void Start()
        {
            m_rouletteCtrl.InstantiateFlashcards(m_learningStageFlashcards.ToArray());
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
            m_learningStageFlashcards = m_learningStageFlashcards.OrderBy(flashCard => new System.Random().Next()).ToList();        
        }
        public void LearnFlashcard()
        {
            if (m_rouletteCtrl.IsAnimating) return;
            FlashcardData flashcard = m_rouletteCtrl.HighlitedFlashcard.FlashcardData;
            LearningStages newStage = flashcard.LearningStage == LearningStages.Acquired ? LearningStages.Acquired : flashcard.LearningStage + 1;
            FlashcardData newFlashcard = new FlashcardData(flashcard.CardFront, flashcard.CardBack, newStage);

            PlayerDataManager.Instance.SetFlashcard(flashcard.CardFront, newFlashcard);

            m_learningStageFlashcards.Remove(flashcard);
            m_rouletteCtrl.RemoveHighlightedFlashcard();

            if (m_learningStageFlashcards.Count == 0) OnAllFlashcardsLearned();
        }
        public void ForgetFlashcard()
        {
            if (m_rouletteCtrl.IsAnimating) return;
            FlashcardData flashcard = m_rouletteCtrl.HighlitedFlashcard.FlashcardData;
            LearningStages newStage = LearningStages.Ignorant;
            FlashcardData newFlashcard = new FlashcardData(flashcard.CardFront, flashcard.CardBack, newStage);

            PlayerDataManager.Instance.SetFlashcard(flashcard.CardFront, newFlashcard);

            m_learningStageFlashcards.Remove(flashcard);
            m_rouletteCtrl.RemoveHighlightedFlashcard();

            if (m_learningStageFlashcards.Count == 0) OnAllFlashcardsLearned();
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
