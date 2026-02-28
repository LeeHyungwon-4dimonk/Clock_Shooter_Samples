using System.Collections.Generic;
using System;
using UnityEngine;

public class ProjectileSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] _directionObjects;

    private readonly List<Func<Vector3, Vector3>> _directionModifiers = new();

    public void AddLeftShot()
    {
        _directionModifiers.Add(dir => Quaternion.Euler(0, -90f, 0) * dir);
        _directionObjects[0].SetActive(true);
    }
    public void AddRightShot()
    {
        _directionModifiers.Add(dir => Quaternion.Euler(0, 90f, 0) * dir);
        _directionObjects[1].SetActive(true);
    }
    public void AddBackShot()
    {
        _directionModifiers.Add(dir => -dir);
        _directionObjects[2].SetActive(true);
    }

    public IEnumerable<Vector3> GetFireDirections(Vector3 baseDir)
    {
        yield return baseDir;

        foreach (var mod in _directionModifiers)
            yield return mod(baseDir);
    }

    public void ResetSystem()
    {
        _directionModifiers.Clear();
        foreach (var obj in _directionObjects)
            obj.SetActive(false);
    }
}