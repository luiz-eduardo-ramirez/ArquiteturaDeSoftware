using ASLeitner.DataStructs;
using ASLeitner.Net;
using Base.Extensions.Attributes;
using Base.Extensions.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ASLeitner.Managers
{
    public class PlayerDataManager : MonoSingleton<PlayerDataManager>
    {
        [SerializeField]
        [ReadOnly]
        private DeckData m_playerDeck;

        private const int k_requestsLimit = 4;


        public string UserID { get => SystemInfo.deviceUniqueIdentifier; }
        public DeckData PlayerDeck { get => m_playerDeck; }
        protected override void Awake()
        {
            base.Awake();

            m_playerDeck = TryToDownloadDeckData();

            //SceneManager.LoadScene(1);
        }

        private IEnumerator RegisterUserOnServer()
        {
            for (int i = 0; i < k_requestsLimit; i++)
            {
                StartCoroutine(ServerComs.SetUserDeckAsync(UserID, new DeckData(),
                    (_result) => { if (_result == UnityWebRequest.Result.Success) i = k_requestsLimit; },
                    (_progress) => Debug.Log("Uploading empty deck: " + (_progress * 100).ToString() + "%")
                    ));
            }
        }


        private DeckData TryToDownloadDeckData()
        {
            DeckData temp = null;

            //Tenta baixar o deck do server
            ServerComs.GetUserDeckAsync(UserID, 
                (_deckData, _result) =>
                {
                    //Caso o processo de baixar o deck ocorra sem erros,
                    //mas o deck continua nulo, o usuario ainda nao esta cadastrado
                    if (_result == UnityWebRequest.Result.Success && _deckData == null)
                    {
                        
                    }
                    else
                        temp = _deckData;
                },
                (_progress) =>
                {
                    Debug.Log("Downloading deck: " + (_progress * 100).ToString() + "%");
                });

            return temp;
        }
    }
}
