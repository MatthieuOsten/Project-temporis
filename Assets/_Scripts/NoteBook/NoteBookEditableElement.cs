using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBookEditableElement : MonoBehaviour
{
    [SerializeField] protected EditIllustrationButton _linckedButton;
    [SerializeField] protected NoteBookManager _noteBookManager;
    [SerializeField] protected Transform _elementToShow;

    protected virtual void OnIllustrationEdited(int index)
    {
        if(CameraManager.Instance.CanLookAt(_elementToShow))
        {
            _noteBookManager.LookUp();
            CameraManager.Instance.LookAt(_elementToShow, 200f);
        }
    }

    /*protected abstract void OnNoteBookOpened();
    protected abstract void OnNoteBookClosed();*/

    protected void Start()
    {
        //_linckedButton.GetIllustrationIndex();
        _linckedButton.illustrationEdited += OnIllustrationEdited;
    }

}
