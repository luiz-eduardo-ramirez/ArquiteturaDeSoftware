using ASLeitner.DataStructs; //temporario para teste
using ASLeitner.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SortingMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_textWarning;

    private void Start()
    {
        m_textWarning.gameObject.SetActive(false);
    }

    private void CheckIgnoranceStage()
    {
        DeckData deckTest = PlayerDataManager.Instance.PlayerDeck; //temporario para teste - substituir pelo server

        int ignorantListSize = PlayerDataManager.Instance.IgnorantStage().Count;
        if(ignorantListSize == 0)
        {
            m_textWarning.gameObject.SetActive(true);
            m_textWarning.text = "Voce nao possui flashcards!";
        }
        else
        {
            Debug.Log("Menu ignorancia inicializado");
            m_textWarning.gameObject.SetActive(false);
        }
    }

    public void CheckSuperficialStage()
    {
        int superficialListSize = PlayerDataManager.Instance.SuperficialStage().Count;
        if (superficialListSize == 0)
        {
            m_textWarning.gameObject.SetActive(true);
            m_textWarning.text = "Voce nao possui flashcards!";
        }
        else
        {
            Debug.Log("Menu superficial inicializado");
            m_textWarning.gameObject.SetActive(false);
        }
    }

    public void CheckAcquiredStage()
    {
        int acquiredListSize = PlayerDataManager.Instance.AcquiredStage().Count;
        if (acquiredListSize == 0)
        {
            m_textWarning.gameObject.SetActive(true);
            m_textWarning.text = "Voce nao possui flashcards!";
        }
        else
        {
            Debug.Log("Menu adquirido inicializado");
            m_textWarning.gameObject.SetActive(false);
        }
    }
}