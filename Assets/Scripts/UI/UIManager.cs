using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private Transform _screenRoot;
    private Transform _popupRoot;

    private readonly Dictionary<System.Type, UIBase> _uiCache = new();
    private readonly Stack<UIBase> _popupStack = new();

    private UIBase _currentScreen;

    public void Initialize()
    {
        //await Manager.Data.WaitForReady();
        FindCanvasAndRoots();
        //await OpenUI<UIIntroScene>(UIType.Screen);

        IsInitialized = true;
    }

    private void FindCanvasAndRoots()
    {
        var canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("[UIManager] Canvas not found in scene.");
            return;
        }

        _screenRoot = canvas.transform.Find("ScreenRoot");
        _popupRoot = canvas.transform.Find("PopupRoot");

        if (_screenRoot == null || _popupRoot == null)
        {
            Debug.LogError("[UIManager] ScreenRoot or PopupRoot not found.");
        }
    }

    public async Task<T> OpenUI<T>(UIType type) where T : UIBase
    {
        var ui = await GetOrCreateUI<T>(type);

        if (ui == null)
            return null;

        if (type == UIType.Screen)
        {
            _currentScreen?.Close();
            _currentScreen = ui;
        }
        else
        {
            _popupStack.Push(ui);
        }

        ui.Open();
        return ui as T;
    }

    public void CloseUI<T>() where T : UIBase
    {
        if (!_uiCache.TryGetValue(typeof(T), out var ui))
            return;

        ui.Close();

        if (ui == _currentScreen)
        {
            _currentScreen = null;
        }
        else if (_popupStack.Count > 0 && _popupStack.Peek() == ui)
        {
            _popupStack.Pop();
        }
    }

    private async Task<UIBase> GetOrCreateUI<T>(UIType type) where T : UIBase
    {
        var key = typeof(T);

        if (_uiCache.TryGetValue(key, out var ui))
            return ui;

        var go = await AssetLoaderProvider.Loader.LoadAsync<GameObject>(key.Name);
        if (go == null)
        {
            Debug.LogError($"[UIManager] Failed to load UI prefab: {key.Name}");
            return null;
        }

        var parent = type == UIType.Screen ? _screenRoot : _popupRoot;
        var instance = Object.Instantiate(go, parent).GetComponent<T>();
        if (instance == null)
        {
            Debug.LogError($"[UIManager] {key.Name} does not have a {typeof(T).Name} component!");
            return null;
        }

        instance.Close();
        _uiCache.Add(key, instance);
        return instance;
    }
}