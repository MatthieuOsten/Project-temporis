using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    private Mouse _virtualMouse;
    private Mouse _currentMouse;

    private Vector2 _deltaValue;
    [SerializeField] float _cursorSpeed = 1000f;
    //[SerializeField] RectTransform _canvasRectTransform;

    [SerializeField] float _padding = 35f;

    private string _previousControlScheme = "";
    private const string _gamepadScheme = "Gamepad";
    private const string _keyboardScheme = "Keyboard";

    private void OnEnable()
    {
        _currentMouse = Mouse.current;
        if(_virtualMouse == null)
        {
            _virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        }
        else if(!_virtualMouse.added)
        {
            InputSystem.AddDevice(_virtualMouse);
        }

        InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);
        InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
        InputSystem.onAfterUpdate += UpdateMotion;
        InputManager.Instance.SubmitStarted += OnSubmitStarted;
        InputManager.Instance.SubmitCanceled += OnSubmitCanceled;
        InputManager.Instance.GamepadCursorPositionPerformed += OnGamepadCursorPositionPerformed;
        InputManager.Instance.GamepadCursorPositionCanceled += OnGamepadCursorPositionCanceled;
    }

    private void OnDisable()
    {
        _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
        InputSystem.RemoveDevice(_virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        Debug.LogWarning("Disable");
    }

    void UpdateMotion()
    {
        Debug.Log("Update");
        if(_virtualMouse == null || Gamepad.current == null || _deltaValue == Vector2.zero)
        {
            return;
        }

        Vector2 newPosition = _virtualMouse.position.ReadValue() + _deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, _padding, Screen.width - _padding);
        newPosition.y = Mathf.Clamp(newPosition.y, _padding, Screen.height - _padding);

        InputState.Change(_virtualMouse.position, newPosition);
        InputState.Change(_virtualMouse.delta, _deltaValue);
    }

    void OnGamepadCursorPositionPerformed(InputAction.CallbackContext context)
    {
        _deltaValue = context.ReadValue<Vector2>();
        _deltaValue *= _cursorSpeed * Time.deltaTime;
    }
    void OnGamepadCursorPositionCanceled(InputAction.CallbackContext context)
    {
        _deltaValue = Vector2.zero;
    }

    void OnSubmitStarted(InputAction.CallbackContext context)
    {
        _virtualMouse.CopyState<MouseState>(out var mouseSate);
        mouseSate.WithButton(MouseButton.Left, true);
        InputState.Change(_virtualMouse, mouseSate);
    }
    void OnSubmitCanceled(InputAction.CallbackContext context)
    {
        _virtualMouse.CopyState<MouseState>(out var mouseSate);
        mouseSate.WithButton(MouseButton.Left, false);
        InputState.Change(_virtualMouse, mouseSate);
    }

    /*void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position,CameraUtility.Camera, out anchoredPosition);
        // Apply position to cursor
    }*/

    /*public void OnControlsChanged(PlayerInput input)
    {
        if(_playerInput.currentControlScheme == _keyboardScheme && _previousControlScheme != _keyboardScheme)
        {
            _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
            _previousControlScheme = _keyboardScheme;
        }
        else if(_playerInput.currentControlScheme == _gamepadScheme && _previousControlScheme != _gamepadScheme)
        {
            InputState.Change(_virtualMouse.position, _currentMouse.position.ReadValue());
            _previousControlScheme = _gamepadScheme;
        }
    }*/
}
