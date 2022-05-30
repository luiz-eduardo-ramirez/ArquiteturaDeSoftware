using ASLeitner.DataStructs;
using ASLeitner.Managers;
using Base.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner
{
    public class DeckEditMenu : MonoBehaviour
    {
        [SerializeField] private MagicalRouletteCtrl m_rouletteCtrl;
        [SerializeField] private GameObject m_inputAbsorver;
        [SerializeField] private GameObject m_flashcardNotFound;
        [SerializeField] private TMP_InputField m_termInputField;
        [SerializeField] private TMP_InputField m_definitionInputField;
        [SerializeField] private TMP_InputField m_searchTermField;

        private void Start()
        {
            m_rouletteCtrl.InstantiateFlashcards(PlayerDataManager.Instance.PlayerDeckToArray());
            m_termInputField.gameObject.SetActive(false);
            m_definitionInputField.gameObject.SetActive(false);
            m_flashcardNotFound.gameObject.SetActive(false);
        }
        public void OnFlashcardEdited()
        {
            if (PlayerDataManager.Instance.DeckSize <= 0 || m_rouletteCtrl.IsAnimating) return;

            Flashcard flashcard = m_rouletteCtrl.HighlitedFlashcard;
            string text;
            string originalKey = flashcard.FlashcardData.CardFront;

            if (m_rouletteCtrl.HighlitedFlashcard.ShowingTerm)
            {
                m_rouletteCtrl.HighlitedFlashcard.SetTermVisibility(true);
                text = m_termInputField.text;
                flashcard.SetFlashCard(new FlashcardData(text, flashcard.FlashcardData.CardBack, flashcard.FlashcardData.LearningStage));
                m_termInputField.gameObject.SetActive(false);
            }
            else
            {
                m_rouletteCtrl.HighlitedFlashcard.SetDefinitionVisibility(true);
                text = m_definitionInputField.text;
                flashcard.SetFlashCard(new FlashcardData(flashcard.FlashcardData.CardFront, text, flashcard.FlashcardData.LearningStage));
                m_definitionInputField.gameObject.SetActive(false);
            }

            PlayerDataManager.Instance.SetFlashcard(originalKey, flashcard.FlashcardData);
        }
        public void OnInsertFlashcard()
        {
            if (m_rouletteCtrl.IsAnimating || PlayerDataManager.Instance.DeckSize >= PlayerDataManager.Instance.MaxDeckSize) return;

            m_rouletteCtrl.InsertNewFlashcard();

            TimersManager.CallAfterConditionIsTrue(
                () =>
                {
                    Flashcard flashcard = m_rouletteCtrl.HighlitedFlashcard;
                    PlayerDataManager.Instance.AddNewFlashcard(flashcard.FlashcardData);
                }, 
                () => !m_rouletteCtrl.IsAnimating);            
        }
        public void OnRemoveFlashcard()
        {
            if (m_rouletteCtrl.IsAnimating) return;

            Flashcard flashcard = m_rouletteCtrl.HighlitedFlashcard;
            PlayerDataManager.Instance.DeleteFlashcard(flashcard.FlashcardData.CardFront);
            m_rouletteCtrl.RemoveHighlightedFlashcard();
        }
        public void BackToMainMenu()
        {
            m_inputAbsorver.SetActive(true);
            PlayerDataManager.Instance.SaveFlashcards(() => SceneManager.LoadScene(SceneRefs.MainMenu));            
        }
        public void OnEditText()
        {
            if (PlayerDataManager.Instance.DeckSize <= 0 || m_rouletteCtrl.IsAnimating) return;

            if (m_rouletteCtrl.HighlitedFlashcard.ShowingTerm)
            {
                m_rouletteCtrl.HighlitedFlashcard.SetTermVisibility(false);
                m_termInputField.gameObject.SetActive(true);
                m_termInputField.ActivateInputField();
            }
            else
            {
                m_rouletteCtrl.HighlitedFlashcard.SetDefinitionVisibility(false);
                m_definitionInputField.gameObject.SetActive(true);
                m_definitionInputField.ActivateInputField();
            }
        }
        public void OnSearchFlashcard()
        {
            string term = m_searchTermField.text;
            FlashcardData flashcard = PlayerDataManager.Instance.GetFlashcard(term);

            if (flashcard != null)
            {
                m_rouletteCtrl.RotateToFlashcard(flashcard);
                m_flashcardNotFound.gameObject.SetActive(false);
            }
            else
            {
                m_flashcardNotFound.gameObject.SetActive(true);
            }
        }
    }
}
