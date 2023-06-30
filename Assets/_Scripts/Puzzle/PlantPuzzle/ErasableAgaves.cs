using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErasableAgaves : NoteBookErasableElement
{
    [SerializeField] GameObject[] _allWeevils;
    [SerializeField] GameObject[] _allPickableAgaves;

    protected override void OnIllustrationErased()
    {
        base.OnIllustrationErased();
        for(int i =  0; i < _allWeevils.Length; i++)
        {
            _allWeevils[i].SetActive(false);
        }
        for(int i = 0; i<_allPickableAgaves.Length; i++)
        {
            _allPickableAgaves[i].layer = 11;
        }
    }
}
