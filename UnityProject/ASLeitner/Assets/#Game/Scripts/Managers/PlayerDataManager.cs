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

            CheckUsrRegistry();

            if(SceneManager.GetActiveScene().name == SceneRefs.Setup)
                SceneManager.LoadScene(SceneRefs.MainMenu);
        }

        private void RegisterUserOnServer(int _requests = 0)
        {
            StartCoroutine(ServerComs.SetUserDeckAsync(UserID, new DeckData(),
                (_result) => 
                {
                    if (_result == UnityWebRequest.Result.Success)
                    {
                        AdvanceToNextScene();
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
                        AdvanceToNextScene();
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
    }
}
