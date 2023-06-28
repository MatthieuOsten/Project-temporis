using System.Collections;
using UnityEngine;

public class RotatingStatuePart : NoteBookEditableElement
{
    [SerializeField] RotateManager _rotateManager;
    [SerializeField] int _partIndex;
 
    protected override void OnIllustrationEdited(int index)
    {
        base.OnIllustrationEdited(index);
        _rotateManager.RotatePartTo(_partIndex, index+1);
    }

    /*public IEnumerator RotateToward(float rotY)
    {
        Quaternion rot = Quaternion.Euler(0, rotY, 0);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 80f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = rot;
    }*/
}
