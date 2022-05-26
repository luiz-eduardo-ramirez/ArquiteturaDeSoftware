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

        private List<FlashcardData> m_ignorantStageList;
        private List<FlashcardData> m_superficialStageList;
        private List<FlashcardData> m_acquiredStageList;
        private Dictionary<string, FlashcardData> m_playerDeckDict;

        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        //private DeckData PlayerDeck { get => m_playerDeck; }
        public int DeckSize { get => m_playerDeckDict.Count; }
        public List<FlashcardData> IgnoranceList { get => m_ignorantStageList; }
        public List<FlashcardData> SuperficialList { get => m_ignorantStageList; }
        public List<FlashcardData> AcquiredList { get => m_ignorantStageList; }

        protected override void Awake()
        {
            base.Awake();

            Debug.Log("Player data manager foi inicializado");

            m_playerDeck = TryToDownloadDeckData();
            m_playerDeckDict = CreateFlashcardDictionary(m_playerDeck);

            if(SceneManager.GetActiveScene().name == SceneRefs.Setup)
                SceneManager.LoadScene(SceneRefs.MainMenu);
        }

        private void CreateLearningStagesSets(DeckData _deck)
        {
            m_ignorantStageList = new List<FlashcardData>();
            m_superficialStageList = new List<FlashcardData>();
            m_acquiredStageList = new List<FlashcardData>();

            foreach (FlashcardData flashcard in _deck.FlashCards)
            {
                switch (flashcard.LearningStage)
                {
                    case LearningStages.Ignorant:
                        m_ignorantStageList.Add(flashcard);
                        break;
                    case LearningStages.Superficial:
                        m_superficialStageList.Add(flashcard);
                        break;
                    case LearningStages.Acquired:
                        m_acquiredStageList.Add(flashcard);
                        break;
                }
            }
        }

        // Apagar depois de fazer conexao com servidor
        private DeckData CreateTestDeck()
        {
            DeckData deckTeste = new DeckData();
            deckTeste.FlashCards = new FlashcardData[60];
            
            for(int i = 0; i < 20; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + i;
                deckTeste.FlashCards[i].CardBack = "abacateBack" + i;
                deckTeste.FlashCards[i].LearningStage = LearningStages.Ignorant;
            }

            for (int i = 20; i < 40; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + i;
                deckTeste.FlashCards[i].CardBack = "abacateBack" + i;
                deckTeste.FlashCards[i].LearningStage = LearningStages.Superficial;
            }

            for (int i = 40; i < 60; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData();
                deckTeste.FlashCards[i].CardFront = "abacateFront" + i;
                deckTeste.FlashCards[i].CardBack = "abacateBack" + i;
                deckTeste.FlashCards[i].LearningStage = LearningStages.Acquired;
            }

            return deckTeste;
        }

        private DeckData TryToDownloadDeckData()
        {
            //codigo maneiro de servidor



            DeckData temp = CreateTestDeck();
            //DeckData temp = new DeckData();

            //temp.FlashCards = new FlashcardData[10];
            //temp.FlashCards[0] = new FlashcardData();
            //temp.FlashCards[0].CardFront = "Brasil";
            //temp.FlashCards[0].CardBack = "Brasilia";
            //temp.FlashCards[0].LearningStage = LearningStages.Ignorant;

            return temp;
        }

        private Dictionary<string, FlashcardData> CreateFlashcardDictionary(DeckData _deck)
        {

            Dictionary<string, FlashcardData> flashcardDict = new Dictionary<string, FlashcardData>();

            foreach(FlashcardData flashcard in _deck.FlashCards)
            {
                flashcardDict.Add(flashcard.CardFront, flashcard);
            }

            return flashcardDict;
        }

        public void SetFlashcard(string _oldKey, FlashcardData _flashcard)
        {
            m_playerDeckDict.Remove(_oldKey);
            m_playerDeckDict.Add(_flashcard.CardFront, _flashcard);
        }

        public FlashcardData GetFlashcard(string _key)
        {            
            return m_playerDeckDict[_key];
        }

        public void AddNewFlashcard(FlashcardData _flashcard)
        {
            m_playerDeckDict.Add(_flashcard.CardFront, _flashcard);
        }

        public void DeleteFlashcard(string _key)
        {
            m_playerDeckDict.Remove(_key);
        }
    }
}
