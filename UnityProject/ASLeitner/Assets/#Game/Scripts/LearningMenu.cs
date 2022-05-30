using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using ASLeitner.DataStructs;
using ASLeitner.Managers;
using System;
using System.Linq;
using Base.Managers;
using TMPro;

namespace ASLeitner
{
    public class LearningMenu : MonoBehaviour
    {
        public static LearningStages CurrentLearningStage { private get; set; }
        public static PlayerDataManager.LearningSets LearningSets { private get; set; }

        [SerializeField] private TextMeshProUGUI m_correctGuesses;
        [SerializeField] private TextMeshProUGUI m_incorrectGuesses;
        [SerializeField] private TextMeshProUGUI m_correctGuessesHeader;
        [SerializeField] private TextMeshProUGUI m_incorrectGuessesHeader;
        [SerializeField] private MagicalRouletteCtrl m_rouletteCtrl;
        [SerializeField] private GameObject m_inputAbsorver;

        private List<FlashcardData> m_learningStageFlashcards;

        private int m_numOfCorrectGuesses;
        private int m_numOfIncorrectGuesses;

        private void Awake()
        {
            BuildLearningDeck();
        }
        private void Start()
        {
            m_rouletteCtrl.InstantiateFlashcards(m_learningStageFlashcards.ToArray());
            m_correctGuessesHeader.gameObject.SetActive(false);
            m_incorrectGuessesHeader.gameObject.SetActive(false);
            m_numOfCorrectGuesses = 0;
            m_numOfIncorrectGuesses = 0;
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

            m_numOfCorrectGuesses++;
            m_learningStageFlashcards.Remove(flashcard);
            m_rouletteCtrl.RemoveHighlightedFlashcard();

            if (m_learningStageFlashcards.Count == 0)
                TimersManager.CallAfterConditionIsTrue(OnAllFlashcardsLearned, () => !m_rouletteCtrl.IsAnimating);
        }
        public void ForgetFlashcard()
        {
            if (m_rouletteCtrl.IsAnimating) return;
            FlashcardData flashcard = m_rouletteCtrl.HighlitedFlashcard.FlashcardData;
            LearningStages newStage = LearningStages.Ignorant;
            FlashcardData newFlashcard = new FlashcardData(flashcard.CardFront, flashcard.CardBack, newStage);

            PlayerDataManager.Instance.SetFlashcard(flashcard.CardFront, newFlashcard);

            m_numOfIncorrectGuesses++;
            m_learningStageFlashcards.Remove(flashcard);
            m_rouletteCtrl.RemoveHighlightedFlashcard();

            if (m_learningStageFlashcards.Count == 0)
                TimersManager.CallAfterConditionIsTrue(OnAllFlashcardsLearned, () => !m_rouletteCtrl.IsAnimating);
        }
        public void BackToMainMenu()
        {
            PlayerDataManager.Instance.ResetFlashcards();
            SceneManager.LoadScene(SceneRefs.MainMenu);
        }
        public void OnAllFlashcardsLearned()
        {
            m_inputAbsorver.SetActive(true);
            m_correctGuessesHeader.gameObject.SetActive(true);
            m_incorrectGuessesHeader.gameObject.SetActive(true);
            m_correctGuesses.text = m_numOfCorrectGuesses.ToString();
            m_incorrectGuesses.text = m_numOfIncorrectGuesses.ToString();
            PlayerDataManager.Instance.SaveFlashcards(() =>
            {
                TimersManager.CallAfterTime(() => SceneManager.LoadScene(SceneRefs.MainMenu), 3.5f);
            });            
        }
    }
}
