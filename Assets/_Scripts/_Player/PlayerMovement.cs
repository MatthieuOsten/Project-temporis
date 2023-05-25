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
    [SerializeField] float _moveSpeed, _sprintSpeed;
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
        StartCoroutine(SmoothStart(_moveSpeed));
        InputManager.Instance.SprintStarted += OnSprintStarted;
        InputManager.Instance.SprintCanceled += OnSprintCanceled;
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
        InputManager.Instance.SprintStarted -= OnSprintStarted;
        InputManager.Instance.SprintCanceled -= OnSprintCanceled;
    }

    IEnumerator SmoothStart(float speed)
    {
        while (_currentMoveSpeed < speed && _isMoving)
        {
            _currentMoveSpeed += Time.deltaTime * 400;
            yield return null;
        }
    }

    public void OnSprintStarted(InputAction.CallbackContext context)
    {
        StartCoroutine(SmoothStart(_sprintSpeed));
    }

    public void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _currentMoveSpeed = _moveSpeed;
    }
}
