using ASLeitner.DataStructs; //temporario para teste
using ASLeitner.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner
{
    public class SortingMenu : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_textWarning;

        private PlayerDataManager.LearningSets m_learningSets;

        private void Awake()
        {
            m_learningSets = PlayerDataManager.Instance.GetLearningStagesSets();
        }
        private void Start()
        {
            m_textWarning.gameObject.SetActive(false);
        }

        private void CheckIgnoranceStage()
        {
            int ignorantListSize = m_learningSets.Ignorance.Count;
            if (ignorantListSize == 0)
            {
                m_textWarning.gameObject.SetActive(true);
                m_textWarning.text = "Voce nao possui flashcards!";
            }
            else
            {
                m_textWarning.gameObject.SetActive(false);
                LearningMenu.LearningSets = m_learningSets;
                LearningMenu.CurrentLearningStage = LearningStages.Ignorant;
                SceneManager.LoadScene(SceneRefs.LearningStage);
                Debug.Log("Menu ignorancia inicializado");
            }
        }

        public void CheckSuperficialStage()
        {
            int superficialListSize = m_learningSets.Superficial.Count;
            if (superficialListSize == 0)
            {
                m_textWarning.gameObject.SetActive(true);
                m_textWarning.text = "Voce nao possui flashcards!";
            }
            else
            {
                m_textWarning.gameObject.SetActive(false);
                LearningMenu.LearningSets = m_learningSets;
                LearningMenu.CurrentLearningStage = LearningStages.Superficial;
                SceneManager.LoadScene(SceneRefs.LearningStage);
                Debug.Log("Menu superficial inicializado");
            }
        }

        public void CheckAcquiredStage()
        {
            int acquiredListSize = m_learningSets.Acquired.Count;
            if (acquiredListSize == 0)
            {
                m_textWarning.gameObject.SetActive(true);
                m_textWarning.text = "Voce nao possui flashcards!";
            }
            else
            {
                m_textWarning.gameObject.SetActive(false);
                LearningMenu.LearningSets = m_learningSets;
                LearningMenu.CurrentLearningStage = LearningStages.Acquired;
                SceneManager.LoadScene(SceneRefs.LearningStage);
                Debug.Log("Menu adquirido inicializado");
            }
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(SceneRefs.MainMenu);
        }
    }
}