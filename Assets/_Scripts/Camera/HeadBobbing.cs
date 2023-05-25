using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBobbing : MonoBehaviour
{
    private float _sinTime = 0;
    [SerializeField, Range(0, 10)] private float _walkingEffectSpeed, _idleEffectSpeed, _pushingEffectSpeed, _sprintEffectSpeed;
    [SerializeField, Range(0, 1)] private float _walkingEffectIntensity, _idleEffectIntensity, _pushingEffectIntensity, _sprintEffectIntensity;
    private float _currentEffectSpeed, _currentEffectIntensity;
    private float _originalOffset;
    private bool _isMoving;
    private bool _isSprinting;
    public bool isPushing;
    private bool _isMovingPushing;
    [SerializeField] AnimationCurve _animationCurve;

    // Start is called before the first frame update
    void Start()
    {
        _originalOffset = transform.position.y;
        _currentEffectSpeed = _idleEffectSpeed;
        _currentEffectIntensity = _idleEffectIntensity;
        InputManager.Instance.MoveStarted += OnMoveStarted;
        InputManager.Instance.MoveCanceled += OnMoveCanceled;
        InputManager.Instance.SprintStarted += OnSprintStarted;
        InputManager.Instance.SprintCanceled += OnSprintCanceled;
        _animationCurve.postWrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update()
    {
        _sinTime += Time.deltaTime * _currentEffectSpeed;
        float sinAmountY = -Mathf.Abs(_currentEffectIntensity * Mathf.Sin(_sinTime));
        if (isPushing && _isMovingPushing)
        {
            transform.position = new Vector3(transform.position.x, _originalOffset - _animationCurve.Evaluate(_sinTime) * _currentEffectIntensity, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, _originalOffset + sinAmountY, transform.position.z);
        }
    }

    public void OnMoveStarted(InputAction.CallbackContext context)
    {
        _isMoving = true;
        _sinTime = 0;
        StopAllCoroutines();
        if (_isSprinting)
        {
            StartCoroutine(Timer(0.5f, _sprintEffectSpeed));
            _currentEffectIntensity = _sprintEffectIntensity;
        }
        else
        {
            StartCoroutine(Timer(0.5f, _walkingEffectSpeed));
            _currentEffectIntensity = _walkingEffectIntensity;
        }
    }

    public void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        StopAllCoroutines();
        _currentEffectSpeed = _idleEffectSpeed;
        _currentEffectIntensity = _idleEffectIntensity;
    }
    
    public void OnMoveWhilePushingStarted(InputAction.CallbackContext context)
    {
        _isMovingPushing = true;
        StopAllCoroutines();
        StartCoroutine(Timer(0.5f, _pushingEffectSpeed));
        _currentEffectIntensity = _pushingEffectIntensity;
    }

    public void OnMoveWhilePushingCanceled(InputAction.CallbackContext obj)
    {
        _isMovingPushing = false;
        _currentEffectSpeed = _idleEffectSpeed;
        _currentEffectIntensity = _idleEffectIntensity;
    }

    public void OnSprintStarted(InputAction.CallbackContext context)
    {
        _isSprinting = true;
        if(_isMoving)
        {
            StopAllCoroutines();
            StartCoroutine(Timer(0.5f, _sprintEffectSpeed));
            _currentEffectIntensity = _sprintEffectIntensity;
        }
    }

    private void OnSprintCanceled(InputAction.CallbackContext obj)
    {
        if(_isMoving)
        {
            StopAllCoroutines();
            _currentEffectSpeed = _walkingEffectSpeed;
            _currentEffectIntensity = _walkingEffectIntensity;
        }
        _isSprinting = false;
    }

    IEnumerator Timer(float timer, float effectSpeed)
    {
        float currentTimer = 0;
        while (currentTimer < timer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        _currentEffectSpeed = effectSpeed;
    }
}
