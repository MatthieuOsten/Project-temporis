using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBookEditableElement : MonoBehaviour
{
    [SerializeField] protected EditIllustrationButton _linckedButton;

    protected abstract void OnIllustrationEdited(int index);

    protected void Start()
    {
        _linckedButton.illustrationEdited += OnIllustrationEdited;
    }
}
