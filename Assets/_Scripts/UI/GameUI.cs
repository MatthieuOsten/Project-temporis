using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interact;
    [SerializeField] Image _holdItem;

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
}
