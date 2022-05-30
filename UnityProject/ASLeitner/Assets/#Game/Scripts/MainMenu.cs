using ASLeitner.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_usrId;
        int CardQuantity { get => PlayerDataManager.Instance.DeckSize; }

        private void Start()
        {
            m_usrId.text = PlayerDataManager.Instance.UserID;
        }
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

        public void QuitApplication()
        {
            Application.Quit();
            Debug.Log("Saindo da aplicação...");
        }
    }
}