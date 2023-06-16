using UnityEngine;

[CreateAssetMenu(fileName = "new_EventTriggerScriptableEnviroNoteBook", menuName = "Events/EventTriggerScriptable/EventTriggerScriptableEnviroNoteBook")]
public class EventTriggerScriptableEnviroNoteBook : EventScriptable<Collider>
{
    public string _description;
    public bool _activate;
    public Sprite _draw;
}
