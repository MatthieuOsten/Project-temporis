using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditIllustrationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button _illustrationButton;
    [SerializeField] Button[] _editIllustrationButtons;
    [SerializeField] GameObject _editIllustrationInterface;
    [SerializeField] List<Sprite> _illustrationStates;
    public Action<int> illustrationEdited;
    [SerializeField] NoteBookManager _noteBookManager;

    private void Start()
    {
        _illustrationButton.onClick.AddListener(ShowEditIllustrationInterface);
    }

    void ShowEditIllustrationInterface()
    {
        _illustrationButton.interactable = false;
        _editIllustrationInterface.SetActive(true);
        int indexToIgnore = _illustrationStates.IndexOf(_illustrationButton.image.sprite);
        Debug.Log(indexToIgnore);
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
    }

    void EditIllustration(int index)
    {
        illustrationEdited?.Invoke(index);
        _illustrationButton.image.sprite = _illustrationStates[index];
        CloseEditIllustrationInterface();
    }

    void CloseEditIllustrationInterface()
    {
        _illustrationButton.interactable = true; 
        _editIllustrationInterface.SetActive(false);
        GameUI.Instance.ShowHandCursor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_illustrationButton.interactable)
        {
            GameUI.Instance.ShowPenCursor();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_illustrationButton.interactable)
        {
            GameUI.Instance.ShowHandCursor();
        }
    }
}
