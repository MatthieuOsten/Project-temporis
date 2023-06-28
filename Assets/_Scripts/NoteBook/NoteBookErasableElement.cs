using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBookErasableElement : MonoBehaviour
{
    [SerializeField] protected EraseIllustrationButton _linckedButton;
    [SerializeField] protected NoteBookManager _noteBookManager;
    [SerializeField] protected Transform _elementToShow;

    protected virtual void OnIllustrationErased()
    {
        if (CameraManager.Instance.CanLookAt(_elementToShow))
        {
            _noteBookManager.LookUp();
            CameraManager.Instance.LookAt(_elementToShow, 200f);
        }
    }

    protected void Start()
    {
        _linckedButton.illustrationErased += OnIllustrationErased;
    }
}
