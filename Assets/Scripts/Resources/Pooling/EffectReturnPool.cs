using System.Collections;
using UnityEngine;

public class EffectReturnPool : MonoBehaviour, IPoolable
{
    private string _poolKey;
    private float _duration = 0.5f;
    private Coroutine _returnCoroutine;

    public void SetPoolKey(string key) => _poolKey = key;
    public void SetDuration(float duration) => _duration = duration;

    public void OnSpawned()
    {
        if (_returnCoroutine != null) StopCoroutine(_returnCoroutine);

        _returnCoroutine = StartCoroutine(ReturnCoroutine());
    }

    public void OnReturned()
    {
        if (_returnCoroutine != null)
        {
            StopCoroutine(_returnCoroutine);
            _returnCoroutine = null;
        }
    }

    private IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSecondsRealtime(_duration);
        Manager.Pool.Return(_poolKey, gameObject);
    }
}