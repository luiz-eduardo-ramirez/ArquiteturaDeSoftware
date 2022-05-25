using ASLeitner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner.Menu
{
    public class MainMenu : MonoBehaviour
    {
        int CardQuantity { get => PlayerDataManager.Instance.PlayerDeck.DeckSize; }
        public void SortFlashcard()
        {
            if (CardQuantity >= 10)
            {
                SceneManager.LoadScene("Sorting");
                Debug.Log("Sorteando flashcard!");
            }
        }

        public void InsertRemoveFlashcard()
        {
            SceneManager.LoadScene("InsertRemove");
            Debug.Log("Inserindo/removendo flashcard!"); 
        }

        public void ShareFlashcard()
        {
            if (CardQuantity == 0)
            {
                Debug.Log("Numero minimo de flashcards = 1");
            }
            else
            {
                SceneManager.LoadScene("Share");
                Debug.Log("Compartilhando flashcard!");
            }
        }

        public void QuitApplication()
        {
            Application.Quit();
            Debug.Log("Saindo da aplicação...");
        }
    }
}