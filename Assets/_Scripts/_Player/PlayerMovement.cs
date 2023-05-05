using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _playerRigidbody;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    [SerializeField] float _moveSpeed;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
    }

    private void Start()
    {
        InputManager.Instance.MoveChanged += OnMove;
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = transform.TransformDirection(_currentMovement * _moveSpeed); 
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>(); // Store Input info 
        _currentMovement.x = _currentMovementInput.x; // Set Input value in var
        _currentMovement.y = 0;
        _currentMovement.z = _currentMovementInput.y;
    }
}
