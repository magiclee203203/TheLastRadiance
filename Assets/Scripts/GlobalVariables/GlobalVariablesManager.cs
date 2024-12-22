using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[ShowOdinSerializedPropertiesInInspector]
public class GlobalVariablesManager : Singleton<GlobalVariablesManager>
{
    [SerializeField, Required] private Dictionary<string, IVariableData> _variablesDict;

    private void Awake()
    {
        // Clear Dictionaries
        _variablesDict.Clear();
    }

    public bool GetValue<T>(string key, out T value, bool strictly = false)
    {
        if (_variablesDict.ContainsKey(key))
            return strictly
                ? _variablesDict[key].TryStrictlyGetValue(out value)
                : _variablesDict[key].TryGetValue(out value);

        value = default;
        return false;
    }

    public void SetValue<T>(string key, T value)
    {
        _variablesDict[key] = new VariableData<T>(value);
    }

    public void RemoveValue(string key)
    {
        _variablesDict.Remove(key);
    }

    public bool HasKey(string key) => _variablesDict.ContainsKey(key);
}