using System.Threading.Tasks;
using UnityEngine;

public class ManagerBootstrapper : MonoBehaviour
{
    public static bool IsBootstrapped { get; private set; }

    private async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        await InitializeManagers();
        IsBootstrapped = true;
    }

    private async Task InitializeManagers()
    {
#if ADDRESSABLES_ENABLED
        AssetLoaderProvider.Initialize(new AddressablesAssetLoader());
#else
        //AssetLoaderProvider.Initialize(new ResourcesAssetLoader());
#endif

        //DataManager.CreateInstance();
        //await Manager.Data.InitAllData();

        PoolManager.CreateInstance();

        //AudioManager.CreateInstance();
        //await Manager.Audio.Initialize();

        //SoundManager.CreateInstance();
        //await Manager.Sound.Initialize();

        //UIManager.CreateInstance();
        //await Manager.UI.Initialize();

        StatusManager.CreateInstance();
        //await Manager.Status.Initialize();

        GameManager.CreateInstance();
        await Manager.Game.Initialize();

        //SteamManager.CreateInstance();
        //await Manager.Steam.Initialize();
    }
}