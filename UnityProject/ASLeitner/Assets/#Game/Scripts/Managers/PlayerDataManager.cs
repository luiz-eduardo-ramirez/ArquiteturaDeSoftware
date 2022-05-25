using ASLeitner.DataStructs;
using Base.Extensions.Attributes;
using Base.Extensions.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner.Managers
{
    public class PlayerDataManager : MonoSingleton<PlayerDataManager>
    {
        [SerializeField]
        [ReadOnly]
        private DeckData m_playerDeck;

        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        public DeckData PlayerDeck { get => m_playerDeck; }
        protected override void Awake()
        {
            base.Awake();

            m_playerDeck = TryToDownloadDeckData();

            if(SceneManager.GetActiveScene().name == SceneRefs.Setup)
                SceneManager.LoadScene(SceneRefs.MainMenu);
        }

        private DeckData TryToDownloadDeckData()
        {
            //codigo maneiro de servidor




            DeckData temp = new DeckData();

            //temp.FlashCards = new FlashcardData[10];
            //temp.FlashCards[0] = new FlashcardData();
            //temp.FlashCards[0].CardFront = "Brasil";
            //temp.FlashCards[0].CardBack = "Brasilia";
            //temp.FlashCards[0].LearningStage = LearningStages.Ignorant;

            return temp;
        }
    }
}
