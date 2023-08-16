using UnityEngine;

namespace Core
{
    public class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<T>(true);

                    if(_instance == null)
                    {
                        GameObject gameObject = new();
                        gameObject.name = typeof(T).Name;
                        _instance = gameObject.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        public virtual void Awake() 
        {
            if( _instance == null ) 
            {
                _instance = this as T;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
