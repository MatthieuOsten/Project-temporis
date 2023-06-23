using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditIllustrationInterface : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] EditIllustrationButton _linckedButton;
    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.Instance.ClickCanceled = null; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.Instance.ClickCanceled = _linckedButton.CloseEditIllustrationInterface;
    }
}
