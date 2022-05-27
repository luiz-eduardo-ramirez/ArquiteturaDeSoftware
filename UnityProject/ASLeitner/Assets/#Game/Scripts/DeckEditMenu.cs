using ASLeitner.Managers;
using Base.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner
{
    public class DeckEditMenu : MonoBehaviour
    {
        [SerializeField] private MagicalRouletteCtrl m_rouletteCtrl;
        [SerializeField] private GameObject m_inputAbsorver;

        private void Start()
        {
            m_rouletteCtrl.InstantiateFlashcards(PlayerDataManager.Instance.PlayerDeckToArray());
        }
        public void OnFlashCardEdited()
        {
            if (m_rouletteCtrl.IsAnimating) return;

            Flashcard flashcard = m_rouletteCtrl.HighlitedFlashcard;
            PlayerDataManager.Instance.SetFlashcard(flashcard.FlashcardData.CardFront, flashcard.FlashcardData);
        }
        public void OnFlashCardInserted()
        {
            if (m_rouletteCtrl.IsAnimating) return;

            m_rouletteCtrl.InsertNewFlashcard();

            TimersManager.CallAfterConditionIsTrue(
                () =>
                {
                    Flashcard flashcard = m_rouletteCtrl.HighlitedFlashcard;
                    PlayerDataManager.Instance.AddNewFlashcard(flashcard.FlashcardData);
                }, 
                () => !m_rouletteCtrl.IsAnimating);            
        }
        public void OnFlashCardRemoved()
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
    }
}
