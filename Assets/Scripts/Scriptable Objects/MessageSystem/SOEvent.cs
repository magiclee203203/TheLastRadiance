using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOEvent", menuName = "Scriptable Object/Events/Event Without Parameters")]
public class SOEvent : ScriptableObject
{
    private readonly List<Action> _subscribers = new();

    public void Subscribe(Action subscriber)
    {
        if (!_subscribers.Contains(subscriber))
        {
            _subscribers.Add(subscriber);
        }
    }

    public void Unsubscribe(Action subscriber)
    {
        if (_subscribers.Contains(subscriber))
        {
            _subscribers.Remove(subscriber);
        }
    }

    public void Notify()
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Invoke();
        }
    }
}

public abstract class SOEvent<T> : ScriptableObject
{
    private readonly List<Action<T>> _subscribers = new();

    public void Subscribe(Action<T> subscriber)
    {
        if (!_subscribers.Contains(subscriber))
        {
            _subscribers.Add(subscriber);
        }
    }

    public void Unsubscribe(Action<T> subscriber)
    {
        if (_subscribers.Contains(subscriber))
        {
            _subscribers.Remove(subscriber);
        }
    }

    public void Notify(T param)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Invoke(param);
        }
    }
}