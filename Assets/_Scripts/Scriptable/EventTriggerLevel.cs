using UnityEngine;


[CreateAssetMenu(fileName = "new_EventTriggerScriptableLevel", menuName = "Events/EventTriggerScriptable/EventTriggerScriptableLevel")]
public class EventTriggerLevel : EventScriptable<Collider>
{
    public AudioClip _audio;
}
