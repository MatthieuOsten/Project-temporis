using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAccesToMaïsField : MonoBehaviour
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] GameObject _accessToMaïsField;

    // Update is called once per frame
    void Update()
    {
        if (_lakeState.IsCorrect)
            _accessToMaïsField.SetActive(!_accessToMaïsField.activeInHierarchy);
        else
            _accessToMaïsField.SetActive(true);
    }
}
