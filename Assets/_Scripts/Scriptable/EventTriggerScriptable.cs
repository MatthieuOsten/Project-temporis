using UnityEngine;

[CreateAssetMenu(fileName = "new_EventTriggerScriptable", menuName = "Events/EventTriggerScriptable")]
public class EventTriggerScriptable : EventScriptable<Collider>
{
    public string _description;
    public bool _activate;
    public Sprite _draw;
}
