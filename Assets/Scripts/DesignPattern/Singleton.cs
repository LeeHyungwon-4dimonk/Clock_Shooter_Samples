using System.Threading.Tasks;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance { get { return _instance; } protected set { _instance = value; } }

    public bool IsInitialized { get; protected set; }

    public static T CreateInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject(typeof(T).Name);
            _instance = go.AddComponent<T>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }

    public static void ReleaseInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }

    public async Task WaitForReady()
    {
        while (!IsInitialized)
            await Task.Yield();
    }
}