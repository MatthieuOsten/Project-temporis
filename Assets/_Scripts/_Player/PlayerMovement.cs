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

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.y = 0;
        _currentMovement.z = _currentMovementInput.y;
        _playerRigidbody.velocity = transform.TransformDirection(_currentMovement * _moveSpeed); 
    }
}
