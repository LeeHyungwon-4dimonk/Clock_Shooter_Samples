using System.Threading.Tasks;
using UnityEngine;

public class ResourcesAssetLoader : IAssetLoader
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task<T> LoadAsync<T>(string key) where T : Object
    {
        var asset = Resources.Load<T>(key);
        return await Task.FromResult(asset);
    }

    public async Task<T[]> LoadAllAsync<T>(string key) where T : Object
    {
        var asset = Resources.LoadAll<T>(key);
        return await Task.FromResult(asset);
    }

    public void Release<T>(T asset) where T : Object
    {

    }
}