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

        private List<FlashcardData> ignorantStageList = new List<FlashcardData>();
        private List<FlashcardData> superficialStageList = new List<FlashcardData>();
        private List<FlashcardData> acquiredStageList = new List<FlashcardData>();

        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        public DeckData PlayerDeck { get => m_playerDeck; }
        protected override void Awake()
        {
            base.Awake();

            m_playerDeck = TryToDownloadDeckData();

            SceneManager.LoadScene(1);
        }

        private void SearchLearningStages(DeckData _deck)
        {
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
        }

        public List<FlashcardData> ignorantStage()
        {
            return ignorantStageList;
        }

        public List<FlashcardData> superficialStage()
        {
            return superficialStageList;
        }

        public List<FlashcardData> acquiredStage()
        {
            return acquiredStageList;
        }

        // Apagar depois de fazer conexao com servidor
        public DeckData createTestDeck()
        {
            DeckData deckTeste = new DeckData();
            
            for(int i = 0; i < 20; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + i;
                deckTeste.FlashCards[i].CardBack = "abacateBack" + i;
                deckTeste.FlashCards[i].LearningStage = LearningStages.Ignorant;
            }

            for (int i = 0; i < 20; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + (20+i);
                deckTeste.FlashCards[i].CardBack = "abacateBack" + (20+i);
                deckTeste.FlashCards[i].LearningStage = LearningStages.Superficial;
            }

            for (int i = 0; i < 20; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + (40+i);
                deckTeste.FlashCards[i].CardBack = "abacateBack" + (40+i);
                deckTeste.FlashCards[i].LearningStage = LearningStages.Acquired;
            }

            return deckTeste;
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
