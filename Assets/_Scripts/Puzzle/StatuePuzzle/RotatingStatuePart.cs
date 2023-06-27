using System.Collections;
using UnityEngine;

public class RotatingStatuePart : NoteBookEditableElement
{
    protected override void OnNoteBookClosed()
    {
        /*switch(_currentIndex)
        {
            case 0:
                StartCoroutine(RotateToward(0));
                break;
            case 1:
                StartCoroutine(RotateToward(180));
                break;
            case 2:
                StartCoroutine(RotateToward(90));
                break;
            case 3:
                StartCoroutine(RotateToward(-90));
                break;
        }*/
    }

    public IEnumerator RotateToward(float rotY)
    {
        Quaternion rot = Quaternion.Euler(0, rotY, 0);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 80f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = rot;
    }
}
