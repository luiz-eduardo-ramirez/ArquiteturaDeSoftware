using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASLeitner
{
    [CreateAssetMenu(fileName = "SceneRefs", menuName = "ScriptableObjects/SceneRefs", order = 1)]
    public class SceneRefs : ScriptableObject
    {
        private static SceneRefs s_instance;

        [SerializeField]
        private string m_insertRemove;
        [SerializeField]
        private string m_mainMenu;
        [SerializeField]
        private string m_setup;
        [SerializeField]
        private string m_share;
        [SerializeField]
        private string m_sorting;


        private static SceneRefs Instance 
        {
            get 
            {
                if (s_instance == null)
                {
                    s_instance = Resources.Load(typeof(SceneRefs).Name) as SceneRefs;
                }
                return s_instance; 
            }
        }

        public static string InsertRemove { get => Instance.m_insertRemove; }
        public static string MainMenu { get => Instance.m_mainMenu; }
        public static string Setup { get => Instance.m_setup; }
        public static string Share { get => Instance.m_share; }
        public static string Sorting { get => Instance.m_sorting; }
    }
}
