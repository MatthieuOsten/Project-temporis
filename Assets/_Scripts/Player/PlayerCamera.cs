using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private float _xRotation = 0f;
    
    private float _xSens, _ySens;
    [SerializeField, Range(0f, 10f)] private float _xSensMouse, _ySensMouse;
    [SerializeField, Range(0f, 100f)] private float _xSensGamepad, _ySensGamepad;

    private float _mouseX;
    private float _mouseY;

    private void OnCameraEnabled()
    {
        Cursor.lockState = CursorLockMode.Locked;
        this.enabled = true;
    }

    private void OnCameraDisabled()
    {
        Cursor.lockState = CursorLockMode.Confined;
        this.enabled = false;
    }

    private void OnControlSchemeSwitched(ControlSchemeState currentControlScheme)
    {
        switch(currentControlScheme)
        {
            case ControlSchemeState.keyboard:
                _xSens = _xSensMouse;
                _ySens = _ySensMouse;
                break;
            case ControlSchemeState.gamepad:
                _xSens = _xSensGamepad;
                _ySens = _ySensGamepad;
                break;
        }
    }

    private void Start()
    {
        _mainCamera = CameraUtility.Camera;
        _xSens = _xSensMouse;
        _ySens = _ySensMouse;
        InputManager.Instance.CameraChanged += RotatePlayer;
        InputManager.Instance.CameraDisabled += OnCameraDisabled;
        InputManager.Instance.CameraEnabled += OnCameraEnabled;
        InputManager.Instance.ControlSchemeSwitched += OnControlSchemeSwitched;
        Cursor.lockState = CursorLockMode.Locked;
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

        // rotate player to look left and right
        gameObject.transform.Rotate(Vector3.up * (_mouseX * Time.deltaTime) * _xSens);

        // apply this to our camera transform
        _mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }
}

