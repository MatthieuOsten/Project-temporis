using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region SINGELTON
    static InputManager _instance;
    static public InputManager Instance
    {
        get
        {
            if( _instance == null )
            {
                GameObject newInputManager = new GameObject("InputManager");
                _instance = newInputManager.AddComponent<InputManager>();
                PlayerInput playerInput = newInputManager.AddComponent<PlayerInput>();
                playerInput.actions.FindAction("Move").performed += _instance.OnMoveChanged;
                playerInput.actions.FindAction("Interact").performed += _instance.OnInteractChanged;
                playerInput.actions.FindAction("Camera").performed += _instance.OnCameraChanged;
                playerInput.actions.FindAction("OpenNoteBook").performed += _instance.OnOpenNoteBookChanged;
                _instance._playerInput = playerInput;
                return _instance;
            }
            else
            {
                return _instance;
            }
        }
        set
        {
            if(Instance != null)
            {
                Destroy(value.gameObject);
            }
        }
    }
    #endregion

    [SerializeField] protected PlayerInput _playerInput;

    private void Awake()
    {
        Instance = this;
        if(_playerInput == null )
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }

    #region MOVE

    public Action<InputAction.CallbackContext> MoveChanged;

    public void OnMoveChanged(InputAction.CallbackContext context)
    {
        MoveChanged?.Invoke(context);
    }
    #endregion

    #region INTERACT

    public Action<InputAction.CallbackContext> InteractStarted;
    public Action<InputAction.CallbackContext> InteractCancelled;

    public void OnInteractChanged(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            InteractStarted?.Invoke(context);
        }
        else if(context.canceled)
        {
            InteractCancelled?.Invoke(context);
        }
    }
    #endregion

    #region CAMERA

    public Action<InputAction.CallbackContext> CameraChanged;

    public void OnCameraChanged(InputAction.CallbackContext context)
    {
        CameraChanged?.Invoke(context);
    }
    #endregion

    #region NOTEBOOK

    public Action<InputAction.CallbackContext> OpenNoteBookChanged;

    public void OnOpenNoteBookChanged(InputAction.CallbackContext context)
    {
        OpenNoteBookChanged?.Invoke(context);
    }
    #endregion

    #region ACTIONS UTILITIES

    public void EnableAllActions()
    {
        _playerInput.actions.FindAction("Move").Enable();
        _playerInput.actions.FindAction("Interact").Enable();
        _playerInput.actions.FindAction("Camera").Enable();
        _playerInput.actions.FindAction("OpenNoteBook").Enable();

    }
    public void DisableAllActions()
    {
        _playerInput.actions.FindAction("Move").Disable();
        _playerInput.actions.FindAction("Interact").Disable();
        _playerInput.actions.FindAction("Camera").Disable();
        _playerInput.actions.FindAction("OpenNoteBook").Disable();

    }
    public void EnableActions(bool move, bool interact, bool camera, bool openNoteBook)
    {
        if (move)
        {
            _playerInput.actions.FindAction("Move").Enable();
        }
        if (interact)
        {
            _playerInput.actions.FindAction("Interact").Enable();
        }
        if (camera)
        {
            _playerInput.actions.FindAction("Camera").Enable();
        }
        if (openNoteBook)
        {
            _playerInput.actions.FindAction("OpenNoteBook").Enable();
        }
    }
    public void DisableActions(bool move, bool interact, bool camera, bool openNoteBook)
    {
        if (move)
        {
            _playerInput.actions.FindAction("Move").Disable();
        }
        if (interact)
        {
            _playerInput.actions.FindAction("Interact").Disable();
        }
        if(camera)
        {
            _playerInput.actions.FindAction("Camera").Disable();
        }
        if (openNoteBook)
        {
            _playerInput.actions.FindAction("OpenNoteBook").Disable();
        }
    }
    #endregion
}
