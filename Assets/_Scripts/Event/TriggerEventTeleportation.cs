using UnityEngine;
using System;
using System.Collections;

public class TriggerEventTeleportation : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Transform _spawn;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _teleportationUI;

    private void EventTeleportation()
    {
        _teleportationUI.SetActive(!_teleportationUI.activeInHierarchy);
        _animator.SetBool("HitTrigger", true);
        InputManager.Instance.DisableAllInGameActions();
        StartCoroutine(DisableUI());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            EventTeleportation();
        }
    }

    public void SetPlayerPosition()
    {
        _player.transform.position = _spawn.position;
        InputManager.Instance.EnableAllInGameActions();
    }

    IEnumerator DisableUI()
    {
        yield return new WaitForSeconds(0.5f);
        SetPlayerPosition();
        yield return new WaitForSeconds(1f);
        _teleportationUI.SetActive(!_teleportationUI.activeInHierarchy);
    }
}
