using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightReflector : MonoBehaviour
{
    public Action<LightReflector> rotModified;
    float moveY;
    [HideInInspector] public float direction;
    float _rotateSpeed = 20, _currentRotateSpeed = 0, _startRotateSpeed = 1;

    private void Update()
    {
        if (moveY != 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + moveY * Time.deltaTime * _currentRotateSpeed, transform.rotation.eulerAngles.z));
            rotModified?.Invoke(this);
        }
    }

    public void RotateReflectorStarted(InputAction.CallbackContext context)
    {
        moveY = context.ReadValue<Vector2>().y * direction;
        _currentRotateSpeed = _startRotateSpeed;
        StartCoroutine(SmoothStart());
    }

    public void RotateReflectorCanceled(InputAction.CallbackContext context)
    {
        moveY = 0;
        _currentRotateSpeed = 0;
    }

    public void Reset()
    {
        moveY = 0;
        direction = 0;
        _currentRotateSpeed = 0;
    }

    IEnumerator SmoothStart()
    {
        while (_currentRotateSpeed < _rotateSpeed)
        {
            _currentRotateSpeed += Time.deltaTime * 10;
            yield return null;
        }
    }
}
