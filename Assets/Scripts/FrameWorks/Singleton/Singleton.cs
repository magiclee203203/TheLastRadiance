using Sirenix.Serialization;
using UnityEngine;

public class Singleton<T> : MonoBehaviour, ISerializationCallbackReceiver,
    ISupportsPrefabSerialization where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new();
    private static string ClassName => $"{typeof(T).Name}(Singleton)";

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            lock (_lock)
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance != null)
                {
                    _instance.gameObject.name = ClassName;
                    DontDestroyOnLoad(_instance);
                    return _instance;
                }

                var singleton = new GameObject { name = ClassName };
                _instance = singleton.AddComponent<T>();
                DontDestroyOnLoad(singleton);
                return _instance;
            }
        }
    }

    # region Used for Odin Serialization

    [SerializeField, HideInInspector] private SerializationData serializationData;

    SerializationData ISupportsPrefabSerialization.SerializationData
    {
        get => serializationData;
        set => serializationData = value;
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
    }

    #endregion
}