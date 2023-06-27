using UnityEngine;

[CreateAssetMenu(fileName = "new_EventTriggerScriptableTeleportation", menuName = "Events/EventTriggerScriptable/EventTriggerScriptableTeleportation")]
public class EventTriggerTeleportation : EventScriptable<Collider>
{
    public GameObject _player;
    public Transform _spawn;
}
