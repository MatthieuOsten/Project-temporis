using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EraseIllustrationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button illustrationButton;
    public Action illustrationErased;
    [SerializeField] NoteBookManager _noteBookManager;

    private void Start()
    {
        illustrationButton.onClick.AddListener(EraseIllustration);
    }

    void EraseIllustration()
    {
        illustrationButton.gameObject.SetActive(false);
        illustrationErased?.Invoke();
        GameUI.Instance.ShowHandCursor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (illustrationButton.interactable)
        {
            GameUI.Instance.ShowErasorCursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (illustrationButton.interactable)
        {
            GameUI.Instance.ShowHandCursor();
        }
    }
}
