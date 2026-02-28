using System.Collections.Generic;
using UnityEngine;

public class WeightedRandom<T>
{
    private Dictionary<T, int> _dic = new();

    public void Add(T item, int value)
    {
        if (value < 0)
        {
            Debug.LogError("Value Under 0 can't be added");
            return;
        }

        if (_dic.ContainsKey(item))
        {
            _dic[item] += value;
        }
        else
        {
            _dic.Add(item, value);
        }
    }

    public void Sub(T item, int value)
    {
        if (value < 0)
        {
            Debug.LogError("Value under 0 can't be substracted");
            return;
        }

        if (_dic.ContainsKey(item))
        {
            if (_dic[item] > value)
            {
                _dic[item] -= value;
            }
            else
            {
                Remove(item);
            }
        }
    }

    public void Remove(T item)
    {
        if (_dic.ContainsKey(item))
        {
            _dic.Remove(item);
        }
        else
        {
            Debug.LogError($"{item} is not exist");
        }
    }

    public int GetTotalWeight()
    {
        int totalWeight = 0;

        foreach (int value in _dic.Values)
        {
            totalWeight += value;
        }

        return totalWeight;
    }

    public T GetRandom()
    {
        int total = GetTotalWeight();
        int pivot = Random.Range(0, total);

        int sum = 0;
        foreach (var pair in _dic)
        {
            sum += pair.Value;
            if (pivot < sum)
                return pair.Key;
        }
        return default;
    }

    public Dictionary<T, int> GetList()
    {
        return _dic;
    }

    public Dictionary<T, float> GetPercent()
    {
        Dictionary<T, float> _percentDic = new Dictionary<T, float>();
        float totalWeight = GetTotalWeight();

        foreach (var item in _dic)
        {
            _percentDic.Add(item.Key, item.Value / totalWeight);
        }

        return _percentDic;
    }

    public void ClearList()
    {
        _dic.Clear();
    }
}