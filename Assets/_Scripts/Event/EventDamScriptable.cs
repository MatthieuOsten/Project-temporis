using UnityEngine;

[CreateAssetMenu(fileName = "new_EventDamScriptable", menuName = "Events/EventDamScriptable")]
public class EventDamScriptable : EventScriptable<bool>
{
    public bool _completed;
}
