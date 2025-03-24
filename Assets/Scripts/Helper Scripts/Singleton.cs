using UnityEngine;

/// <summary>
/// The Singleton class is a generic class that can be used to create a singleton instance of a MonoBehaviour.
/// This is needed to ensure that only one instance of a MonoBehaviour is created and that it persists between scenes.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
