using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirror : NoteBookEditableElement
{
    public Action<ReflectingMirror> rotModified;

    override protected void OnIllustrationEdited(int index)
    {
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

    void RotateMirror(float rotY)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * rotY);
    }
}
