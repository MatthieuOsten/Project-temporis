using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTemple : MonoBehaviour
{
    private int _damOpen;
    private float _speed;
    private float _distance = 5f;
    private Vector3 _direction = Vector3.down;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;


    private void Start()
    {
        _speed = Time.deltaTime;
        _initialPosition = transform.position;
        _targetPosition = _initialPosition + (_direction.normalized * _distance);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed);
    }

    public void OpenTempleDoor()
    {
        if (_damOpen == 6)
        {
            //va bouger la porte
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed);
        }
        else
            _damOpen++;
    }
}
