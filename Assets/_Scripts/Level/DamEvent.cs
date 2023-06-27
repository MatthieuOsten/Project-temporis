using UnityEngine;

public class DamEvent : MonoBehaviour
{
    [SerializeField] EventDamScriptable _eventDam;
    [SerializeField] Animator _animator;

    private void Awake()
    {
        _eventDam.Event += OpenDam;
    }

    public void OpenDam(bool openDam)
    {
        _animator.SetBool("Open", true);
    }
}
