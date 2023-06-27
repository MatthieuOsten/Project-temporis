using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    #region SINGELTON
    static GameUI _instance;
    static public GameUI Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newGameUI = new GameObject("GameUI");
                _instance = newGameUI.AddComponent<GameUI>();
                return _instance;
            }
            else
            {
                return _instance;
            }
        }
        set
        {
            if (Instance != null)
            {
                Destroy(value.gameObject);
            }
        }
    }
    #endregion

    [SerializeField] GameObject _playerScreen, _noteBookScreen;
    [Header("INTERACT")]
    [SerializeField] TextMeshProUGUI _interact;
    [Header("INVENTORY")]
    [SerializeField] Image _holdItem;
    [Header("NOTEBOOK")]
    [SerializeField] RectTransform _canvasRectTransform;
    [SerializeField] GameObject _mouseHandCursor, _mousePencilCursor, _mouseErasorCursor, _gamepadHandCursor, _gamepadPencilCursor, _gamepadErasorCursor;
    GameObject _currentHandCursor, _currentPencilCursor, _currentErasorCursor;
    [SerializeField] Transform _cursorHolder;
    public bool isLocked;
    [SerializeField] GamepadCursor _gamepadCursor;
    ControlSchemeState _currentControlScheme;
    CursorState _currentCursor;

    private void Awake()
    {
        _instance = this;
        _currentHandCursor = _mouseHandCursor;
        _currentPencilCursor = _mousePencilCursor;
        _currentErasorCursor = _mouseErasorCursor;
    }

    private void Start()
    {
        _currentCursor = CursorState.none;
        Cursor.visible = false;
        InputManager.Instance.PointPerformed += MoveCursor;
        InputManager.Instance.ControlSchemeSwitched += OnControlSchemeSwitched;
    }

    void OnControlSchemeSwitched(ControlSchemeState currentControlScheme)
    {
        switch(currentControlScheme)
        {
            case ControlSchemeState.gamepad :
                if (_currentCursor != CursorState.none)
                {
                    _gamepadCursor.enabled = true;
                    switch (_currentCursor)
                    {
                        case CursorState.hand:
                            _mouseHandCursor.SetActive(false);
                            _gamepadHandCursor.SetActive(true);
                            break;
                        case CursorState.pencil:
                            _mousePencilCursor.SetActive(false);
                            _gamepadPencilCursor.SetActive(true);
                            break;
                        case CursorState.erasor:
                            _mouseErasorCursor.SetActive(false);
                            _gamepadErasorCursor.SetActive(true);
                            break;
                    }
                }
                _currentHandCursor = _mouseHandCursor;
                _currentPencilCursor = _mousePencilCursor;
                _currentErasorCursor = _mouseErasorCursor;
                break;
            case ControlSchemeState.keyboard:
                if (_currentCursor != CursorState.none)
                {
                    _gamepadCursor.enabled = false;
                    switch (_currentCursor)
                    {
                        case CursorState.hand:
                            _gamepadHandCursor.SetActive(false);
                            _mouseHandCursor.SetActive(true);
                            break;
                        case CursorState.pencil:
                            _gamepadPencilCursor.SetActive(false);
                            _mousePencilCursor.SetActive(true);
                            break;
                        case CursorState.erasor:
                            _gamepadErasorCursor.SetActive(false);
                            _mouseErasorCursor.SetActive(true);
                            break;
                    }
                }
                _currentHandCursor = _gamepadHandCursor;
                _currentPencilCursor = _gamepadPencilCursor;
                _currentErasorCursor = _gamepadErasorCursor;
                break;
        }
        _currentControlScheme = currentControlScheme;
    }

    /// <summary>
    /// Set the visibility of the interact text
    /// </summary>
    /// <param name="set"></param>
    public void ShowInteractText(string text)
    {
        _interact.gameObject.SetActive(true);
        _interact.text = text;
    }

    public void HideInteractText()
    {
        _interact.gameObject.SetActive(false);
    }

    public void ShowItem(Sprite itemToShow)
    {
        _holdItem.gameObject.SetActive(true);
        _holdItem.sprite = itemToShow;
    }
    public void HideItem()
    {
        _holdItem.gameObject.SetActive(false);
        _holdItem.sprite = null;
    }

    public void ShowPlayerScreen()
    {
        _playerScreen.SetActive(true);
    }

    public void HidePlayerScreen()
    {
        _playerScreen.SetActive(false);
    }

    public void ShowNoteBookScreen()
    {
        if(_currentControlScheme == ControlSchemeState.gamepad)
        {
            _gamepadCursor.enabled = true;
        }
        _noteBookScreen.SetActive(true);
    }
    public void HideNoteBookScreen()
    {
        if(_currentControlScheme == ControlSchemeState.gamepad && _currentCursor == CursorState.none)
        {
            _gamepadCursor.enabled = false;
        }
        _noteBookScreen.SetActive(false);
    }

    public void ShowHandCursor()
    {
        if(!isLocked)
        {
            switch(_currentCursor)
            {
                case CursorState.none:
                    _cursorHolder.gameObject.SetActive(true);
                    break;
                case CursorState.hand:
                    return;
                case CursorState.pencil:
                    _currentPencilCursor.SetActive(false);
                    break;
                case CursorState.erasor:
                    _currentErasorCursor.SetActive(false);
                    break;
            }
            _currentCursor = CursorState.hand;
            _currentHandCursor.SetActive(true);
        }
    }

    public void ShowPencilCursor()
    {
        if(!isLocked)
        {
            switch (_currentCursor)
            {
                case CursorState.none:
                    _cursorHolder.gameObject.SetActive(true);
                    break;
                case CursorState.hand:
                    _currentHandCursor.SetActive(false);
                    break;
                case CursorState.pencil:
                    return;
                case CursorState.erasor:
                    _currentErasorCursor.SetActive(false);
                    break;
            }
            _currentCursor = CursorState.pencil;
            _currentPencilCursor.SetActive(true);
        }
    }

    public void ShowErasorCursor()
    {
        if (!isLocked)
        {
            switch (_currentCursor)
            {
                case CursorState.none:
                    _cursorHolder.gameObject.SetActive(true);
                    break;
                case CursorState.hand:
                    _currentHandCursor.SetActive(false);
                    break;
                case CursorState.pencil:
                    _currentPencilCursor.SetActive(false);
                    break;
                case CursorState.erasor:
                    return;
            }
            _currentCursor = CursorState.hand;
            _currentErasorCursor.SetActive(true);
        }
    }

    public void HideCursor()
    {
        switch (_currentCursor)
        {
            case CursorState.none:
                return;
            case CursorState.hand:
                _currentHandCursor.SetActive(false);
                break;
            case CursorState.pencil:
                _currentPencilCursor.SetActive(false);
                break;
            case CursorState.erasor:
                _currentErasorCursor.SetActive(false);
                break;
        }
        _currentCursor = CursorState.none;
        _cursorHolder.gameObject.SetActive(false);
    }

    void MoveCursor(InputAction.CallbackContext context)
    {
        _cursorHolder.transform.position = context.ReadValue<Vector2>();
    }
}
