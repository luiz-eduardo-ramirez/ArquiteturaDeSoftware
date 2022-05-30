using ASLeitner.DataStructs;
using ASLeitner.Net;
using Base.Extensions.Attributes;
using Base.Extensions.Patterns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Networking;
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

        private const int k_requestsLimit = 4;
        private const int k_maxNumOfFlashcards = 50;


        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        //private DeckData PlayerDeck { get => m_playerDeck; }
        public int DeckSize { get => m_playerDeckDict.Count; }
        public int MaxDeckSize { get => k_maxNumOfFlashcards; }

        protected override void Awake()
        {
            base.Awake();

            Debug.Log("Player data manager foi inicializado");
            m_playerDeckDict = new Dictionary<string, FlashcardData>();
            CheckUsrRegistry();
            //m_playerDeck = CreateTestDeck();
            //ResetFlashcards();
            //SaveFlashcards(() => { });

            //if(SceneManager.GetActiveScene().name == SceneRefs.Setup)
            //    SceneManager.LoadScene(SceneRefs.MainMenu);
        }

        // Apagar depois de fazer conexao com servidor
        private DeckData CreateTestDeck()
        {
            DeckData deckTeste = new DeckData();
            deckTeste.FlashCards = new FlashcardData[10];
            for (int i = 0; i < 10; i++)
            {
                deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Ignorant);
            }
            //for(int i = 0; i < 20; i++)
            //{
            //    deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Ignorant);
            //}
            //
            //for (int i = 20; i < 40; i++)
            //{
            //    deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Superficial);
            //}
            //
            //for (int i = 40; i < 60; i++)
            //{
            //    deckTeste.FlashCards[i] = new FlashcardData("abacateFront" + i, "abacateBack" + i, LearningStages.Acquired);
            //}

            return deckTeste;
        }

        private void RegisterUserOnServer(int _requests = 0)
        {
            StartCoroutine(ServerComs.SetUserDeckAsync(UserID, new DeckData(),
                (_result) => 
                {
                    if (_result == UnityWebRequest.Result.Success)
                    {
                        if (SceneManager.GetActiveScene().name == SceneRefs.Setup) SceneManager.LoadScene(SceneRefs.MainMenu);
                    }
                    else
                    {
                        Debug.LogError("Nao foi possivel registrar o usuario");
                        if (++_requests < k_requestsLimit) RegisterUserOnServer(_requests);
                        else Application.Quit();
                    }
                },
                (_progress) => Debug.Log("Uploading empty deck: " + (_progress * 100).ToString() + "%")
                ));
        }

        private void CheckUsrRegistry()
        {
            StartCoroutine(ServerComs.GetUsersIdsAsync(
                (_usrsIds, _result) =>
                {
                    bool usrIdFound = false;
                    if (_result == UnityWebRequest.Result.Success)
                    {
                        if (_usrsIds != null)
                        {
                            for(int i = 0; i < _usrsIds.Length; i++)
                            {
                                if (_usrsIds[i] == UserID)
                                {
                                    usrIdFound = true;
                                    TryToDownloadDeckData();
                                    break;
                                }
                            }
                        }
                        if (!usrIdFound)
                        {
                            RegisterUserOnServer();
                        }
                    }
                },
                (_progress) =>
                {
                    Debug.Log("Downloading UsersIDs: " + (_progress * 100).ToString() + "%");
                }));
        }
        private void TryToDownloadDeckData(int _requests = 0)
        {
            //Tenta baixar o deck do server
            StartCoroutine(ServerComs.GetUserDeckAsync(UserID, 
                (_deckData, _result) =>
                {
                    if (_result == UnityWebRequest.Result.Success)
                    {
                        m_playerDeck = _deckData;
                        m_playerDeckDict = CreateFlashcardDictionary(m_playerDeck);
                        if(SceneManager.GetActiveScene().name == SceneRefs.Setup) SceneManager.LoadScene(SceneRefs.MainMenu);
                    }
                    else
                    {
                        Debug.LogError("Nao foi possivel baixar o deck");
                        if (++_requests < k_requestsLimit) TryToDownloadDeckData(_requests);
                        else Application.Quit();
                    }
                },
                (_progress) =>
                {
                    Debug.Log("Downloading deck: " + (_progress * 100).ToString() + "%");
                }));
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
        public string GetAvailableDefaultKey()
        {
            string candidateKey = "Termo";
            for (int i = 1; i <= k_maxNumOfFlashcards; i++)
            {
                candidateKey = "Termo " + i.ToString();
                if (!m_playerDeckDict.ContainsKey(candidateKey)) break;
            }

            return candidateKey;
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
        public FlashcardData[] PlayerDeckToArray()
        {
            return DictionaryToDeck(m_playerDeckDict).FlashCards;
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

            StartCoroutine(ServerComs.SetUserDeckAsync(UserID, m_playerDeck,
                (_result) =>
                {
                    if (_result == UnityWebRequest.Result.Success)
                    {
                        _onUploadCompleted();
                    }
                    else
                    {
                        Debug.LogError("Nao foi possivel registrar o usuario");
                        Application.Quit();
                    }
                },
                (_progress) => Debug.Log("Uploading empty deck: " + (_progress * 100).ToString() + "%")
                ));

            
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
