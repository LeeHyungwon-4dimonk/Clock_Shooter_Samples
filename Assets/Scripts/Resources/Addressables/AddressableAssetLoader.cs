#if ADDRESSABLES_ENABLED
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;

public class AddressablesAssetLoader : IAssetLoader
{
    public async Task InitializeAsync()
    {
        await Addressables.InitializeAsync().Task;
    }

    public async Task<T> LoadAsync<T>(string key) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        return await handle.Task;
    }

    public async Task<T[]> LoadAllAsync<T>(string key) where T : Object
    {
        var handle = Addressables.LoadAssetsAsync<T>(key, null);
        await handle.Task;
        return handle.Result.ToArray();
    }

    public void Release<T>(T asset) where T : UnityEngine.Object
    {
        Addressables.Release(asset);
    }
}
#endif