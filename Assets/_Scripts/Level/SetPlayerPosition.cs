using UnityEngine;

public class SetPlayerPosition : MonoBehaviour
{
    [SerializeField] EventTriggerTeleportation _eventTrigger;

    public void SetPositionPlayer()
    {
        _eventTrigger._player.transform.position = _eventTrigger._spawn.position;
        InputManager.Instance.EnableAllInGameActions();
    }
}
