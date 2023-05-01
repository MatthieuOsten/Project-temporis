using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightReflector : MonoBehaviour
{
    public Action<LightReflector> rotModified;
    Quaternion currentRot;

    private void Start()
    {
        currentRot = transform.rotation;
    }

    private void Update()
    {
        if(transform.rotation != currentRot)
        {
            rotModified?.Invoke(this);
            currentRot = transform.rotation;
        }
    }
}
