using ASLeitner.DataStructs;
using Base.Extensions.Attributes;
using Base.Extensions.Patterns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASLeitner.Managers
{
    public class PlayerDataManager : MonoSingleton<PlayerDataManager>
    {
        public class LearningSets
        {
            public ReadOnlyCollection<FlashcardData> Ignorance;
            public ReadOnlyCollection<FlashcardData> Superficial;
            public ReadOnlyCollection<FlashcardData> Acquired;
        }

        [SerializeField]
        [ReadOnly]
        private DeckData m_playerDeck;
        private Dictionary<string, FlashcardData> m_playerDeckDict;

        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        //private DeckData PlayerDeck { get => m_playerDeck; }
        public int DeckSize { get => m_playerDeckDict.Count; }

        protected override void Awake()
        {
            base.Awake();

            Debug.Log("Player data manager foi inicializado");

            m_playerDeck = TryToDownloadDeckData();
            m_playerDeckDict = CreateFlashcardDictionary(m_playerDeck);

            if(SceneManager.GetActiveScene().name == SceneRefs.Setup)
                SceneManager.LoadScene(SceneRefs.MainMenu);
        }
        public LearningSets GetLearningStagesSets()
        {
            LearningSets learningSets = new LearningSets();
            List<FlashcardData> ignorance = new List<FlashcardData>();
            List<FlashcardData> superficial = new List<FlashcardData>();
            List<FlashcardData> acquired = new List<FlashcardData>();


            foreach (FlashcardData flashcard in m_playerDeck.FlashCards)
            {
                switch (flashcard.LearningStage)
                {
                    case LearningStages.Ignorant:
                        ignorance.Add(flashcard);
                        break;
                    case LearningStages.Superficial:
                        superficial.Add(flashcard);
                        break;
                    case LearningStages.Acquired:
                        acquired.Add(flashcard);
                        break;
                    default:
                        throw new Exception("Estagio de aprendizado inexistente");
                }
            }
            learningSets.Ignorance = new ReadOnlyCollection<FlashcardData>(ignorance);
            learningSets.Superficial = new ReadOnlyCollection<FlashcardData>(superficial);
            learningSets.Acquired = new ReadOnlyCollection<FlashcardData>(acquired);

            return learningSets;
        }

        // Apagar depois de fazer conexao com servidor
        private DeckData CreateTestDeck()
        {
            DeckData deckTeste = new DeckData();
            deckTeste.FlashCards = new FlashcardData[60];
            
            for(int i = 0; i < 20; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Ignorant);
            }

            for (int i = 20; i < 40; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Superficial);
            }

            for (int i = 40; i < 60; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Acquired);
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

        private DeckData DictionaryToDeck(Dictionary<string, FlashcardData> _dict)
        {
            DeckData deckData = new DeckData();
            deckData.FlashCards = new FlashcardData[_dict.Count];

            int i = 0;
            foreach (KeyValuePair<string, FlashcardData> keyValue in _dict)
            {
                deckData.FlashCards[i] = keyValue.Value;
                i++;
            }

            return deckData;
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
        /// <summary>
        /// Faz upload do baralho para o server
        /// </summary>
        public void SaveFlashcards(Action _onUploadCompleted)
        {
            m_playerDeck = DictionaryToDeck(m_playerDeckDict);
            _onUploadCompleted();
        }
        /// <summary>
        /// Reseta o dicionario de flashcards para a ultima versao baixada do server
        /// </summary>
        public void ResetFlashcards()
        {
            m_playerDeckDict = CreateFlashcardDictionary(m_playerDeck);

        }
    }
}
