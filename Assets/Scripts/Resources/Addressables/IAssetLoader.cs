using System.Threading.Tasks;
using UnityEngine;

public interface IAssetLoader
{
    Task InitializeAsync();
    Task<T> LoadAsync<T>(string key) where T : Object;
    Task<T[]> LoadAllAsync<T>(string key) where T : Object;
    void Release<T>(T asset) where T : Object;
}