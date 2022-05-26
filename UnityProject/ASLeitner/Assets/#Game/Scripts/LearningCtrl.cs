using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace ASLeitner
{
    public class LearningCtrl : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            SceneManager.LoadScene(SceneRefs.MainMenu);
        }
    }
}
