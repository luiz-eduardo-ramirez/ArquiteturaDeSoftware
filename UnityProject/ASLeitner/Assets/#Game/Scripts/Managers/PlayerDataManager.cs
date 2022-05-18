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
        private FlashcardData[] m_ignorantStageCards;
        private FlashcardData[] m_superficialStageCards;
        private FlashcardData[] m_completeStageCards;

        public const int ignorantStage = 0;
        public const int superficialStage = 1;
        public const int acquiredStage = 2;

        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        public DeckData PlayerDeck { get => m_playerDeck; }
        protected override void Awake()
        {
            base.Awake();

            m_playerDeck = TryToDownloadDeckData();

            SceneManager.LoadScene(1);
        }

        private List<FlashcardData> SearchLearningStages(DeckData _deck, int _stage)
        {
            List<FlashcardData> ignorantStageList = new List<FlashcardData>();
            List<FlashcardData> superficialStageList = new List<FlashcardData>();
            List<FlashcardData> acquiredStageList = new List<FlashcardData>();
            List<FlashcardData> errorList = new List<FlashcardData>();

            foreach (FlashcardData flashcard in _deck.FlashCards)
            {
                switch (flashcard.LearningStage)
                {
                    case LearningStages.Ignorant:
                        ignorantStageList.Add(flashcard);
                        break;
                    case LearningStages.Superficial:
                        superficialStageList.Add(flashcard);
                        break;
                    case LearningStages.Acquired:
                        acquiredStageList.Add(flashcard);
                        break;

                }
            }

            if(_stage == ignorantStage)
            {
                return ignorantStageList;
            }
            else if(_stage == superficialStage)
            {
                return superficialStageList;
            }
            else if(_stage == acquiredStage)
            {
                return acquiredStageList;
            }
            else
            {
                return errorList;
            }
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
