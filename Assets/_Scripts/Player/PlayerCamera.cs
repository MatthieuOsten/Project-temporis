using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private float _xRotation = 0f;
    [SerializeField] float _yRotation = 0f;

    [SerializeField] private float _xSens = 30f;
    [SerializeField] private float _ySens = 30f;
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

    private void Start()
    {
        _mainCamera = CameraUtility.Camera;
        InputManager.Instance.CameraChanged += RotatePlayer;
        InputManager.Instance.CameraDisabled += OnCameraDisabled;
        InputManager.Instance.CameraEnabled += OnCameraEnabled;
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

