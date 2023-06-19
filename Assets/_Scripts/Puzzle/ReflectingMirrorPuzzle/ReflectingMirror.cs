using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirror : MonoBehaviour
{
    [SerializeField] ReflectingMirrorPuzzle _reflectingMirrorPuzzle;
    public IEnumerator RotateToward(float rotY)
    {
        _reflectingMirrorPuzzle.BlockRay();
        Quaternion rot = Quaternion.Euler(0, rotY, 0);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 80f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = rot;
        _reflectingMirrorPuzzle.ResetRay();
    }
}
