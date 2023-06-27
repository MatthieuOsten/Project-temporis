using System;
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

    [SerializeField] PlayerInput _playerInput;
    ControlSchemeState _currentControlScheme;

    private void Awake()
    {
        _instance = this;
        if(_playerInput == null )
        {
            _playerInput = GetComponent<PlayerInput>();
        }
        _moveEnabled = true;
        _sprintEnabled = true;
        _interactEnabled = true;
        _cameraEnabled = true;
        _openNoteBookEnabled = true;
        _openInventoryEnabled = true;
    }

    public Action<InputAction.CallbackContext> GameRestarted;

    public void OnGameRestarted(InputAction.CallbackContext context)
    {
        GameRestarted?.Invoke(context);
    }

    public Action<ControlSchemeState> ControlSchemeSwitched;
    public void OnControlSchemeSwitched(PlayerInput input)
    {
        _currentControlScheme = (ControlSchemeState)Enum.Parse(typeof(ControlSchemeState), _playerInput.currentControlScheme.ToLower());
        Debug.Log(_currentControlScheme.ToString());
        ControlSchemeSwitched?.Invoke(_currentControlScheme);
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

    private bool _moveEnabled;
    public bool moveEnabled
    {
        get { return _moveEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("Move").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("Move").Disable();
            }
            _moveEnabled = value;
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

    private bool _sprintEnabled;
    public bool sprintEnabled
    {
        get { return _sprintEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("Sprint").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("Sprint").Disable();
            }
            _sprintEnabled = value;
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

    private bool _interactEnabled;
    public bool interactEnabled
    {
        get { return _interactEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("Interact").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("Interact").Disable();
            }
            _interactEnabled = value;
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

    private bool _cameraEnabled;
    public bool cameraEnabled
    {
        get { return _cameraEnabled; }
        set
        {
            if(value)
            {
                _playerInput.actions.FindAction("Camera").Enable();
                CameraEnabled?.Invoke();
            }
            else
            {
                _playerInput.actions.FindAction("Camera").Disable();
                CameraDisabled?.Invoke();
            }
            _cameraEnabled = value;
        }
    }
    #endregion

    #region OPEN NOTEBOOK

    public Action<InputAction.CallbackContext> OpenNoteBookStarted;

    public void OnOpenNoteBookChanged(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            OpenNoteBookStarted?.Invoke(context);
        }
    }

    private bool _openNoteBookEnabled;
    public bool openNoteBookEnabled
    {
        get { return _openNoteBookEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("OpenNoteBook").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("OpenNoteBook").Disable();
            }
            _openNoteBookEnabled = value;
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

    private bool _openInventoryEnabled;
    public bool openInventoryEnabled
    {
        get { return _openInventoryEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("OpenInventory").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("OpenInventory").Disable();
            }
            _openInventoryEnabled = value;
        }
    }

    #endregion
    #endregion

    #region UI
    #region SUBMIT

    public Action<InputAction.CallbackContext> SubmitStarted;
    public Action<InputAction.CallbackContext> SubmitCanceled;

    public void OnSubmitChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SubmitStarted?.Invoke(context);
        }
        else if(context.canceled)
        {
            SubmitCanceled?.Invoke(context);
        }
    }

    #endregion

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

    #region CLOSE NOTEBOOK

    public Action<InputAction.CallbackContext> CloseNoteBookStarted;
    public void OnCloseNoteBookChanged(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CloseNoteBookStarted?.Invoke(context);
        }
    }

    private bool _closeNoteBookEnabled;
    public bool closeNoteBookEnabled
    {
        get { return _closeNoteBookEnabled; }
        set
        {
            if (value)
            {
                _playerInput.actions.FindAction("CloseNoteBook").Enable();
            }
            else
            {
                _playerInput.actions.FindAction("CloseNoteBook").Disable();
            }
            _closeNoteBookEnabled = value;
        }
    }

    #endregion

    #region GAMEPAD CURSOR POSITION

    public Action<InputAction.CallbackContext> GamepadCursorPositionPerformed;
    public Action<InputAction.CallbackContext> GamepadCursorPositionCanceled;

    public void OnGamepadCursorPositionChanged(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            GamepadCursorPositionPerformed?.Invoke(context);
        }
        else if(context.canceled)
        {
            GamepadCursorPositionCanceled?.Invoke(context);
        }
    }

    #endregion
    #endregion

    #region ACTIONS UTILITIES

    public void EnableAllInGameActions()
    {
        moveEnabled = true;
        sprintEnabled = true;
        interactEnabled = true;
        cameraEnabled = true;
        openNoteBookEnabled = true;
        openInventoryEnabled = true;
    }
    public void DisableAllInGameActions()
    {
        moveEnabled = false;
        sprintEnabled = false;
        interactEnabled = false;
        cameraEnabled = false;
        openNoteBookEnabled = false;
        openInventoryEnabled = false;
    }

    public void SwitchCurrentActionMap()
    {
        switch(_playerInput.currentActionMap.name)
        {
            case "Game":
                DisableAllInGameActions();
                _playerInput.SwitchCurrentActionMap("UI");
                break;
            case "UI":
                _playerInput.SwitchCurrentActionMap("Game");
                EnableAllInGameActions();
                break;
        }
    }
    #endregion
}
