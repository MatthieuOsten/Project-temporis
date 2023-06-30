using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPositionEvent : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Transform _spawn;

    public void SetPlayerPosition()
    {
        _player.transform.position = _spawn.position;
        InputManager.Instance.EnableAllInGameActions();
    }
}
