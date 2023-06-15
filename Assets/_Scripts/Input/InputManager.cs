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

    PlayerInput _playerInput;

    private void Awake()
    {
        _instance = this;
        if(_playerInput == null )
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }

    public Action<InputAction.CallbackContext> GameRestarted;

    public void OnGameRestarted(InputAction.CallbackContext context)
    {
        GameRestarted?.Invoke(context);
    }

    #region GAME
    #region MOVE

    public Action<InputAction.CallbackContext> MoveStarted;
    public Action<InputAction.CallbackContext> MovePerformed;
    public Action<InputAction.CallbackContext> MoveCanceled;

    public void OnMoveChanged(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            MoveStarted?.Invoke(context);
        }
        else if (context.performed)
        {
            MovePerformed?.Invoke(context);
        }
        else if (context.canceled)
        {
            MoveCanceled?.Invoke(context);
        }
    }
    #endregion

    #region SPRINT

    public Action<InputAction.CallbackContext> SprintStarted;
    public Action<InputAction.CallbackContext> SprintCanceled;

    public void OnSprintChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SprintStarted?.Invoke(context);
        }
        else if (context.canceled)
        {
            SprintCanceled?.Invoke(context);
        }
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

    public Action CameraEnabled, CameraDisabled;

    #endregion

    #region NOTEBOOK

    public Action<InputAction.CallbackContext> OpenNoteBookStarted;
    public Action<InputAction.CallbackContext> CloseNoteBookStarted;

    public void OnOpenNoteBookChanged(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OpenNoteBookStarted?.Invoke(context);
        }
    }
    public void OnCloseNoteBookChanged(InputAction.CallbackContext context)
    {
        Debug.Log("CC");
        if (context.started)
        {
            CloseNoteBookStarted?.Invoke(context);
        }
    }
    #endregion

    #region INVENTORY

    public Action<InputAction.CallbackContext> OpenInventoryStarted;
    public Action<InputAction.CallbackContext> CloseInventoryStarted;

    public void OnOpenInventoryChanged(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OpenInventoryStarted?.Invoke(context);
        }
    }

    public void OnCloseInventoryChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CloseInventoryStarted?.Invoke(context);
        }
    }

    #endregion
    #endregion

    #region UI
    #region POINT

    public Action<InputAction.CallbackContext> PointPerformed;

    public void OnPointChanged(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PointPerformed?.Invoke(context);
        }
    }

    #endregion

    #region CLICK

    public Action<InputAction.CallbackContext> ClickCanceled;

    public void OnClickChanged(InputAction.CallbackContext context)
    {
        ClickCanceled?.Invoke(context);
    }

    #endregion
    #endregion

    #region ACTIONS UTILITIES

    public void EnableAllInGameActions()
    {
        _playerInput.SwitchCurrentActionMap("Game");
        CameraEnabled?.Invoke();
    }
    public void DisableAllInGameActions()
    {
        _playerInput.SwitchCurrentActionMap("UI");
        CameraDisabled?.Invoke();
    }
    public void InGameActionsEnabled(bool move, bool interact, bool camera, bool openNoteBook)
    {
        if (move)
        {
            _playerInput.actions.FindAction("Move").Enable();
        }
        else
        {
            _playerInput.actions.FindAction("Move").Disable();
        }

        if (interact)
        {
            _playerInput.actions.FindAction("Interact").Enable();
        }
        else
        {
            _playerInput.actions.FindAction("Interact").Disable();
        }

        if (camera)
        {
            _playerInput.actions.FindAction("Camera").Enable();
            CameraEnabled?.Invoke();
        }
        else
        {
            _playerInput.actions.FindAction("Camera").Disable();
            CameraDisabled?.Invoke();
        }

        if (openNoteBook)
        {
            _playerInput.actions.FindAction("OpenNoteBook").Enable();
        }
        else
        {
            _playerInput.actions.FindAction("OpenNoteBook").Disable();
        }
    }

    #endregion
}
