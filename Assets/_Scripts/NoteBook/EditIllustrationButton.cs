using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditIllustrationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button illustrationButton;
    [SerializeField] Button[] _editIllustrationButtons;
    [SerializeField] GameObject _editIllustrationInterface;
    [SerializeField] List<Sprite> _illustrationStates;
    public Action<int> illustrationEdited;
    [SerializeField] NoteBookManager _noteBookManager;

    private void Start()
    {
        illustrationButton.onClick.AddListener(ShowEditIllustrationInterface);
    }

    void ShowEditIllustrationInterface()
    {
        GameUI.Instance.ShowPenCursor();
        illustrationButton.interactable = false;
        _editIllustrationInterface.SetActive(true);
        int indexToIgnore = _illustrationStates.IndexOf(illustrationButton.image.sprite);
        int buttonIndex = 0;
        for (int i =0; i <_illustrationStates.Count; i++)
        {
            if(i != indexToIgnore)
            {
                int whyTheFuckNot = i;
                _editIllustrationButtons[buttonIndex].onClick.RemoveAllListeners();
                _editIllustrationButtons[buttonIndex].onClick.AddListener(() => EditIllustration(whyTheFuckNot));
                _editIllustrationButtons[buttonIndex].image.sprite = _illustrationStates[i];
                buttonIndex++;
            }
        }
        InputManager.Instance.ClickCanceled = CloseEditIllustrationInterface;
        InputManager.Instance.CloseNoteBookStarted += CloseEditIllustrationInterface;
        GameUI.Instance.isLocked = true;
    }

    public int GetIllustrationIndex()
    {
        return _illustrationStates.IndexOf(illustrationButton.image.sprite);
    }

    void EditIllustration(int index)
    {
        illustrationEdited?.Invoke(index);
        illustrationButton.image.sprite = _illustrationStates[index];
        CloseEditIllustrationInterface();
    }

    public void CloseEditIllustrationInterface()
    {
        _editIllustrationInterface.SetActive(false);
        GameUI.Instance.ShowHandCursor();
    }

    public void CloseEditIllustrationInterface(InputAction.CallbackContext context)
    {
        illustrationButton.interactable = true;
        _editIllustrationInterface.SetActive(false);
        GameUI.Instance.isLocked = false;
        GameUI.Instance.ShowHandCursor();
        InputManager.Instance.ClickCanceled = null;
        InputManager.Instance.CloseNoteBookStarted -= CloseEditIllustrationInterface;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(illustrationButton.interactable)
        {
            GameUI.Instance.ShowPenCursor();
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
