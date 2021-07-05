using UnityEngine;

public class monoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    //private static T m_Instance;
    //public static T Instance
    //{
    //    get
    //    {
    //        if (m_Instance == null)
    //        {
    //            m_Instance = new T();
    //        }
    //        return m_Instance;
    //    }
    //}
    //private static string rootName = "monoSingleton";
    private static GameObject monoSingletionRoot;
    private static T instance;
    public static T Instance
    {
        get
        {
            if (monoSingletionRoot == null)
            {
                monoSingletionRoot = GameObject.Find(typeof(T).ToString());
                if (monoSingletionRoot == null)
                {
                    monoSingletionRoot = new GameObject(typeof(T).ToString());
                }
                DontDestroyOnLoad(monoSingletionRoot);
            }
            if (instance == null)
            {
                instance = monoSingletionRoot.GetComponent<T>();
                if (instance == null)
                {
                    instance = monoSingletionRoot.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
