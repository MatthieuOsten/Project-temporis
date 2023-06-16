using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirror2D : NoteBookEditableElement
{
    public Action<ReflectingMirror2D> rotModified;
    [SerializeField] ReflectingMirror _linckedReflectingMirror;

    override protected void OnIllustrationEdited(int index)
    {
        base.OnIllustrationEdited(index);
        switch(index)
        {
            case 0:
                RotateMirror(0);
                break;
            case 1:
                RotateMirror(90);
                break;
            case 2:
                RotateMirror(45);
                break;
            case 3:
                RotateMirror(-45);
                break;
        }
        rotModified?.Invoke(this);
    }

    protected override void OnNoteBookClosed()
    {
        switch (_currentIndex)
        {
            case 0:
                StartCoroutine(_linckedReflectingMirror.RotateToward(0));
                break;
            case 1:
                StartCoroutine(_linckedReflectingMirror.RotateToward(90));
                break;
            case 2:
                StartCoroutine(_linckedReflectingMirror.RotateToward(-45));
                break;
            case 3:
                StartCoroutine(_linckedReflectingMirror.RotateToward(45));
                break;
        }
    }

    void RotateMirror(float rotY)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * rotY);
    }
}
