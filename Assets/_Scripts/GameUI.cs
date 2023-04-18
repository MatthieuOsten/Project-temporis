using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _interact;

    public void SetInteractText(bool set)
    {
        _interact.gameObject.SetActive(set);
    }
}
