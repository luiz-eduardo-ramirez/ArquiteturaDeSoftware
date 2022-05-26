using ASLeitner.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner.Menu
{
    public class MainMenu : MonoBehaviour
    {
        int CardQuantity { get => PlayerDataManager.Instance.DeckSize; }
        public void SortFlashcard()
        {
            if (CardQuantity >= 10)
            {
                SceneManager.LoadScene(SceneRefs.Sorting);
                Debug.Log("Sorteando flashcard!");
            }
        }

        public void LearningStage()
        {
            if(CardQuantity >= 10)
            {
                SceneManager.LoadScene(SceneRefs.LearningStage);
                Debug.Log("Etapa de aprendizado!");
            }
        }

        public void InsertRemoveFlashcard()
        {
            SceneManager.LoadScene(SceneRefs.InsertRemove);
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
                //SceneManager.LoadScene(SceneRefs.InsertRemove);
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