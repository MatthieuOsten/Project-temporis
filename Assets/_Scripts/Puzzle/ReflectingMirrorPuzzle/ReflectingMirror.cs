using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirror : MonoBehaviour
{
    public ReflectingMirrorPuzzle reflectingMirrorPuzzle;
    public Action<ReflectingMirror> rotModified;

    public IEnumerator RotateToward(float rotY)
    {
        reflectingMirrorPuzzle.LockAllMirrorsButtons();
        Quaternion rot = Quaternion.Euler(0, rotY, 0);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 100f * Time.deltaTime);
            rotModified?.Invoke(this);
            yield return null;
        }
        yield return StartCoroutine(CheckRot(rot));
        rotModified?.Invoke(this);
        reflectingMirrorPuzzle.UnlockAllMirrorsButtons();
    }

    IEnumerator CheckRot(Quaternion rot)
    {
        transform.rotation = rot;
        yield return null;
    }
}
