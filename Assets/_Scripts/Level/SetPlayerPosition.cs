using UnityEngine;

public class SetPlayerPosition : MonoBehaviour
{
    [SerializeField] TriggerEventTeleportation _eventTrigger;

    public void SetPositionPlayer()
    {
        _eventTrigger.Event._player.transform.position = _eventTrigger.Event._spawn.position;
        InputManager.Instance.EnableAllInGameActions();
    }
}
