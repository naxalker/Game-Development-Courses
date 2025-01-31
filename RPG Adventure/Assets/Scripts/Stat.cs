using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int _baseValue;
    private List<int> _modifiers = new List<int>();

    public int Value => _baseValue + _modifiers.Sum();

    public void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        _modifiers.Remove(modifier);
    }
}
