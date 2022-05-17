using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Patterns
{
    public abstract class MonoSingleton<SingletonClass> : MonoInstance where SingletonClass : MonoSingleton<SingletonClass>
    {
        [SerializeField] private bool m_dontDestroyOnLoad;
        
        private static SingletonClass s_instance;

        public static SingletonClass Instance 
        {
            get 
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<SingletonClass>();
                }
                if (s_instance == null) Debug.LogWarning("Nenhuma instancia do objeto encontrada, retornando null");
                return s_instance;
            }
        }

        public bool WillNotDestroyOnLoad { get { return m_dontDestroyOnLoad; } }

        protected virtual void Awake()
        {
            if (s_instance != null) 
            {
                Debug.LogError("More than one instance of "+ name +" found!!!");
                Destroy(gameObject);
                return;
            }
            s_instance = (SingletonClass)this;
            if(m_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
}
