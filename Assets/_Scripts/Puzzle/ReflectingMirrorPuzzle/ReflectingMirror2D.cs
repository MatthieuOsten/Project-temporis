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
                StartCoroutine(_linckedReflectingMirror.RotateToward(0));
                break;
            case 1:
                RotateMirror(90);
                StartCoroutine(_linckedReflectingMirror.RotateToward(90));
                break;
            case 2:
                RotateMirror(-45);
                StartCoroutine(_linckedReflectingMirror.RotateToward(45));
                break;
            case 3:
                RotateMirror(45);
                StartCoroutine(_linckedReflectingMirror.RotateToward(-45));
                break;
        }
    }

    protected override void OnNoteBookClosed()
    {
        
    }

    void RotateMirror(float rotY)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * rotY);
        StartCoroutine(OnRotModified());
        IEnumerator OnRotModified()
        {
            yield return new WaitForSeconds(0.01f);
            rotModified?.Invoke(this);
        }
    }
}
