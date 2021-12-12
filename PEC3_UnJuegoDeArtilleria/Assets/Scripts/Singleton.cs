using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance;

    protected void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
