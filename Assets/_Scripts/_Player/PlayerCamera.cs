using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _xRotation = 0f;
    [SerializeField] float _yRotation = 0f;

    [SerializeField] private float _xSens = 30f;
    [SerializeField] private float _ySens = 30f;
    private float _mouseX;
    private float _mouseY;

    [HideInInspector] public bool isXRotClamped;

    private void Start()
    {
        isXRotClamped = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.CameraChanged += RotatePlayer;
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
        _xRotation = Mathf.Clamp(_xRotation, -60f, 70f);

        if(isXRotClamped)
        {
            _yRotation += (_mouseX * Time.deltaTime) * _xSens;
            _yRotation = Mathf.Clamp(_yRotation, -110, 110);
            CameraUtility.Camera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }
        else
        {
            // apply this to our camera transform
            CameraUtility.Camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            // rotate player to look left and right
            gameObject.transform.Rotate(Vector3.up * (_mouseX * Time.deltaTime) * _xSens);
        }
    }
}

