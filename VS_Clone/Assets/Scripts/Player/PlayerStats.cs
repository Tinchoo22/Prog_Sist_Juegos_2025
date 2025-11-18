using System.Collections.Generic;
using UnityEngine;


public class PlayerStats
{
    
    private Dictionary<string, float> _values = new Dictionary<string, float>();

  
    public float Get(string statId)
    {
        if (string.IsNullOrEmpty(statId)) return 0f;
        return _values.TryGetValue(statId, out var v) ? v : 0f;
    }

   
    public void Add(string statId, float delta)
    {
        if (string.IsNullOrEmpty(statId)) return;
        float current = Get(statId);
        _values[statId] = current + delta;
        // Debug.Log($"[PlayerStats] {statId}: {current} {(delta >= 0 ? "+" : "")}{delta} = {_values[statId]}");
    }
      
    public void Modify(string statId, float delta)
    {
        Add(statId, delta);
    }

    public void Set(string statId, float value)
    {
        if (string.IsNullOrEmpty(statId)) return;
        _values[statId] = value;
    }
     
    public IReadOnlyDictionary<string, float> GetAll() => _values;
            
    public PlayerBuildMemento Save()
    {
        return new PlayerBuildMemento(_values);
    }
     
    public void Load(PlayerBuildMemento memento)
    {
        if (memento == null) return;
        _values = new Dictionary<string, float>(memento.values);
    }
    public void Clear()
    {
        _values.Clear();
    }
}


