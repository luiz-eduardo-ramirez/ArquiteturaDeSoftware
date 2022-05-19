using ASLeitner.DataStructs; //temporario para teste
using ASLeitner.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingMenu : MonoBehaviour
{
    public void CheckIgnoranceStage()
    {
        DeckData deckTeste = PlayerDataManager.Instance.createTestDeck(); //temporario para teste

        int ignorantListSize = PlayerDataManager.Instance.ignorantStage().Count;
        if(ignorantListSize == 0)
        {

        }
        else
        {

        }
    }
    public void CheckSuperficialStage()
    {
        int superficialListSize = PlayerDataManager.Instance.superficialStage().Count;
        if (superficialListSize == 0)
        {

        }
        else
        {

        }
    }

    public void CheckAcquiredStage()
    {
        int acquiredListSize = PlayerDataManager.Instance.acquiredStage().Count;
        if (acquiredListSize == 0)
        {

        }
        else
        {

        }
    }
}