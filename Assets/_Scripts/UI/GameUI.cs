using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] TextMeshProUGUI _interact;
    [SerializeField] Image _holdItem, _laserPOVCameraOutline;
    [SerializeField] GameObject _playerScreen, _noteBookScreen;
    public Action laserPOVCameraShowed, laserPOVCameraHidded;
    [SerializeField] GameObject _handCursor, _penCursor;
    [SerializeField] Transform _cursorHolder;
    public bool isLocked;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
        InputManager.Instance.PointPerformed += MoveCursor;
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
        _noteBookScreen.SetActive(true);
    }
    public void HideNoteBookScreen()
    {
        _noteBookScreen.SetActive(false);
    }
    public void ShowLaserPOVCameraOutline()
    {
        _laserPOVCameraOutline.gameObject.SetActive(true);
        laserPOVCameraShowed?.Invoke();
    }
    public void HideLaserPOVCameraOutline()
    {
        _laserPOVCameraOutline.gameObject.SetActive(false);
        laserPOVCameraHidded.Invoke();
    }

    public void ShowHandCursor()
    {
        if(!isLocked)
        {
            _penCursor.SetActive(false);
            _handCursor.SetActive(true);
        }
    }

    public void ShowPenCursor()
    {
        if(!isLocked)
        {
            _handCursor.SetActive(false);
            _penCursor.SetActive(true);
        }
    }

    public void ShowErasorCursor()
    {

    }

    void MoveCursor(InputAction.CallbackContext context)
    {
        _cursorHolder.transform.position = context.ReadValue<Vector2>();
    }
}
