using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVisibleConstellations : MonoBehaviour
{
    [SerializeField] GameObject _constellation;

    public void VisibleConstellations()
    {
        _constellation.SetActive(!_constellation.activeInHierarchy);
    }
}
