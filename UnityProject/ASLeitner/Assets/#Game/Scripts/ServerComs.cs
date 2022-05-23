using ASLeitner.DataStructs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ASLeitner.Net
{
    public static class ServerComs
    {
        [Serializable]
        private class PlayerData
        {
            public string usrid;
            public DeckData deck;

            public PlayerData(string _usrid, DeckData _deck)
            {
                usrid = _usrid;
                deck = _deck;
            }
        }
        [Serializable]
        private class ServerGetReponse
        {
            public PlayerData Item;
        }
        private const string s_servrUrl = "https://qc8uyffmda.execute-api.us-east-1.amazonaws.com/items";


        public static IEnumerator GetUserDeckAsync(string _usrID, Action<DeckData, UnityWebRequest.Result> _onDownloadFinished, Action<float> _onDownloadUpdate)
        {
            DeckData deckData = null;
            UnityWebRequest getRequest = UnityWebRequest.Get(s_servrUrl + "/" + _usrID);
            DownloadHandler downloadHan;
            getRequest.SendWebRequest();

            while (!getRequest.isDone)
            {
                _onDownloadUpdate(getRequest.downloadProgress);
                yield return null;
            }

            if (getRequest.error != null)
            {
                Debug.LogError(getRequest.error);

                _onDownloadFinished(deckData, getRequest.result);
            }
            else
            {
                downloadHan = getRequest.downloadHandler;
                
                deckData = JsonUtility.FromJson<ServerGetReponse>(downloadHan.text).Item.deck;

                _onDownloadFinished(deckData, getRequest.result);
            }
        }

        public static IEnumerator SetUserDeckAsync(string _usrID, DeckData _usrData, Action<UnityWebRequest.Result> _onUploadFinished, Action<float> _onUploadUpdate)
        {
            PlayerData playerData = new PlayerData(_usrID, _usrData);
            UnityWebRequest setRequest = UnityWebRequest.Put(s_servrUrl, JsonUtility.ToJson(playerData));

            setRequest.SetRequestHeader("Content-Type", "application/json");
            setRequest.SendWebRequest();

            while (!setRequest.isDone)
            {
                _onUploadUpdate(setRequest.uploadProgress);
                yield return null;
            }

            if (setRequest.error != null)
            {
                Debug.LogError(setRequest.error);
            }

            _onUploadFinished(setRequest.result);
        }

        public static DeckData GetUsrDeck(MonoBehaviour _coroutineCaller, string _usrID)
        {
            DeckData deck = null;
            
            _coroutineCaller.StartCoroutine(GetUserDeckAsync(_usrID, (_deck, _result) => deck = _deck, (downloadProgress) => { }));

            return deck;
        }

        public static void SetUserDeck(MonoBehaviour _coroutineCaller, string _usrID, DeckData _usrData)
        {
            _coroutineCaller.StartCoroutine(SetUserDeckAsync(_usrID, _usrData, (result) => { }, (progress) => { }));
        }
    }
}
