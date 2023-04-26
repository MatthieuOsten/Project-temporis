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
    public void SetInteractText(bool set)
    {
        _interact.gameObject.SetActive(set);
    }
}
