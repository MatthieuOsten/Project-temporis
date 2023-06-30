using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    [SerializeField] float _moveSpeed, _sprintSpeed;
    private float _currentMoveSpeed = 0;
    bool _isSprinting = false;

    bool _isMovementPressed;
    CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _isMovementPressed = false;
    }

    private void Start()
    {
        InputManager.Instance.MoveStarted += OnMoveStarted;
        InputManager.Instance.MovePerformed += OnMovePerformed;
        InputManager.Instance.MoveCanceled += OnMoveCanceled;
        InputManager.Instance.SprintStarted += OnSprintStarted;
        InputManager.Instance.SprintCanceled += OnSprintCanceled;
    }

    private void Update()
    {
        _characterController.SimpleMove(transform.TransformDirection(_currentMovement * _currentMoveSpeed));
    }

    public void OnMoveStarted(InputAction.CallbackContext context)
    {
        _isMovementPressed = true;
        if (_isSprinting)
        {
            StartCoroutine(SmoothStart(_sprintSpeed));
        }
        else
        {
            StartCoroutine(SmoothStart(_moveSpeed));
        }
    }
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
    }
    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _isMovementPressed = false;
        _currentMoveSpeed = 0;
    }

    IEnumerator SmoothStart(float speed)
    {
        while (_currentMoveSpeed < speed && _isMovementPressed)
        {
            _currentMoveSpeed += Time.deltaTime * 10;
            yield return null;
        }
    }

    public void OnSprintStarted(InputAction.CallbackContext context)
    {
        _isSprinting = true;
        if(_isMovementPressed)
        {
            StartCoroutine(SmoothStart(_sprintSpeed));
        }
    }

    public void OnSprintCanceled(InputAction.CallbackContext context)
    {
        _isSprinting = false;
        if(_isMovementPressed)
        {
            _currentMoveSpeed = _moveSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            _characterController.slopeLimit = 100;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            _characterController.slopeLimit = 45;
        }
    }
}
