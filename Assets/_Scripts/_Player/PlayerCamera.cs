using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _xRotation = 0f;

    [SerializeField] private float _xSens = 30f;
    [SerializeField] private float _ySens = 30f;
    private float _mouseX;
    private float _mouseY;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RotatePlayer(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _mouseX = input.x;
        _mouseY = input.y;
    }

    private void Update()
    {

        // calcul camera rotation for looking up and down
        _xRotation -= (_mouseY * Time.deltaTime) * _ySens;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        // apply this to our camera transform
        CameraUtility.Camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // rotate player to look left and right
        gameObject.transform.Rotate(Vector3.up * (_mouseX * Time.deltaTime) * _xSens);
    }
}

