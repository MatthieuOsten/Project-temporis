using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new_ScriptableEvent", menuName = "Events/EventScriptable")]
public class EventScriptable : ScriptableObject
{
    public event Action Event;

    public void EventInvoke() //peut s'écrire aussi : public void EventInvoke() => Event?.Invoke();
    {
        Event?.Invoke();
    }
}

public abstract class EventScriptable<T> : ScriptableObject
{
    public event Action<T> Event;

    public void EventInvoke(T value) //peut s'écrire aussi : public void EventInvoke() => Event?.Invoke();
    {
        Event?.Invoke(value);
    }
}

