using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SortFlashcard()
    {
        SceneManager.LoadScene("Sorting");
        Debug.Log("Sorteando flashcard!");
    }

    public void InsertRemoveFlashcard()
    {
        SceneManager.LoadScene("InsertRemove");
        Debug.Log("Inserindo/removendo flashcard!");
    }

    public void ShareFlashcard()
    {
        SceneManager.LoadScene("Share");
        Debug.Log("Compartilhando flashcard!");
    }

    public void QuitApplication()
    {
        Application.Quit();
        Debug.Log("Saindo da aplicação...");
    }
}
