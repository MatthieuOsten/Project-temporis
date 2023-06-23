using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectEvent : MonoBehaviour
{
    [SerializeField] GameObject _insect;

    public void EraseInsect()
    {
        _insect.SetActive(!_insect.activeInHierarchy);
    }
}
