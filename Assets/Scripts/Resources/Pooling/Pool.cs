using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public readonly string _key;
    public readonly GameObject _prefab;
    public readonly int _size = 10;

    private Queue<GameObject> _objects = new();
    private HashSet<GameObject> _activeObjects = new();
    private Transform _parent;

    public Pool(string key, GameObject prefab, int size)
    {
        _key = key;
        _prefab = prefab;
        _size = size;
    }

    public void Init(Transform root, string childName = null)
    {
        if (!string.IsNullOrEmpty(childName))
        {
            GameObject child = new GameObject(childName);
            child.transform.SetParent(root);
            child.transform.localPosition = Vector3.zero;
            _parent = child.transform;
        }
        else
        {
            _parent = root;
        }

        for (int i = 0; i < _size; i++)
            CreateObject();
    }

    private GameObject CreateObject()
    {
        GameObject obj = GameObject.Instantiate(_prefab, _parent);
        obj.SetActive(false);

        if (obj.TryGetComponent<EffectReturnPool>(out var effect))
            effect.SetPoolKey(_key);

        _objects.Enqueue(obj);
        return obj;
    }

    public GameObject Get()
    {
        return Get(_parent);
    }

    public GameObject Get(Transform parent)
    {
        GameObject obj = _objects.Count > 0
            ? _objects.Dequeue()
            : CreateObject();

        obj.transform.SetParent(parent, false);
        obj.SetActive(true);

        obj.GetComponent<IPoolable>()?.OnSpawned();

        _activeObjects.Add(obj);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.GetComponent<IPoolable>()?.OnReturned();

        obj.transform.SetParent(_parent, false);
        obj.SetActive(false);
        _objects.Enqueue(obj);
    }

    public void ReleaseAll()
    {
        foreach (var obj in _activeObjects.ToArray())
        {
            Return(obj);
        }
    }
}