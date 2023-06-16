using UnityEngine;

public class TriggerEventTeleportation : MonoBehaviour
{
    [SerializeField] EventTriggerTeleportation _event;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _teleportationUI;
    public EventTriggerTeleportation Event
    {
        get { return _event; }
        set { _event = value; }
    }
    private void Awake()
    {
        _event.Event += EventTeleportation;
    }

    private void EventTeleportation(Collider col)
    {
        _teleportationUI.SetActive(!_teleportationUI.activeInHierarchy);
        InputManager.Instance.EnableAllInGameActions();
        _animator.SetBool("HitTrigger", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _event.EventInvoke(other);
        }
    }
}
