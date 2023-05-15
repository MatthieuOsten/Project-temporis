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
    private float _currentMoveSpeed = 10;
    bool _isMoving = false;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
    }

    private void Start()
    {
        InputManager.Instance.MoveStarted += OnMoveStarted;
        InputManager.Instance.MovePerformed += OnMovePerformed;
        InputManager.Instance.MoveCanceled += OnMoveCanceled;
    }

    private void FixedUpdate()
    {
        _playerRigidbody.velocity = transform.TransformDirection(_currentMovement * _currentMoveSpeed * Time.deltaTime);
    }

    public void OnMoveStarted(InputAction.CallbackContext context)
    {
        _isMoving = true;
        _currentMoveSpeed = 10;
        StartCoroutine(SmoothStart());
    }
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>(); // Store Input info 
        _currentMovement.x = _currentMovementInput.x; // Set Input value in var
        _currentMovement.y = 0;
        _currentMovement.z = _currentMovementInput.y;
    }
    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _isMoving = false;
        _currentMoveSpeed = 0;
    }

    IEnumerator SmoothStart()
    {
        while (_currentMoveSpeed < _moveSpeed && _isMoving)
        {
            _currentMoveSpeed += Time.deltaTime * 400;
            yield return null;
        }
    }
}
