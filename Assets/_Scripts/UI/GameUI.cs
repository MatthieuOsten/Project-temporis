using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interact;
    [SerializeField] Image _holdItem, _laserPOVCameraOutline;
    [SerializeField] GameObject _playerScreen, _noteBookScreen;
    public Action laserPOVCameraShowed, laserPOVCameraHidded;

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
}
