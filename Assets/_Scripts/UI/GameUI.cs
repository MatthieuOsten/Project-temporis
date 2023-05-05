using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interact;

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
}
