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
    [SerializeField] NoteBookAudio _noteBookAudio;

    private void Start()
    {
        illustrationButton.onClick.AddListener(EraseIllustration);
    }

    void EraseIllustration()
    {
        illustrationErased?.Invoke();
        _noteBookAudio.EraseClip();
        GameUI.Instance.ShowHandCursor();
        illustrationButton.gameObject.SetActive(false);
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
