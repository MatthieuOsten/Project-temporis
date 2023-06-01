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

    private bool isXRotClamped;
    public bool IsXRotClamped
    {
        get { return isXRotClamped; }
        set
        {
            _yRotation = 0;
            isXRotClamped = value;
            if(!value)
            {
                transform.forward = _mainCamera.transform.parent.forward;
                _mainCamera.transform.parent.forward = transform.forward;
            }
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        InputManager.Instance.InventoryStarted += OnInventoryStarted;
        _mainCamera = CameraUtility.Camera;
        isXRotClamped = false;
        InputManager.Instance.CameraChanged += RotatePlayer;
    }

    private void OnInventoryStarted(InputAction.CallbackContext context)
    {
        this.enabled = !this.enabled;
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
            _yRotation = Mathf.Clamp(_yRotation, -90, 90);
            _mainCamera.transform.parent.localRotation = Quaternion.Euler(0, _yRotation, 0);
        }
        else
        {
            // rotate player to look left and right
            gameObject.transform.Rotate(Vector3.up * (_mouseX * Time.deltaTime) * _xSens);
        }
        // apply this to our camera transform
        _mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
    }
}

