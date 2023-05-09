using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightReflector : MonoBehaviour
{
    public Action<LightReflector> rotModified;
    float moveY;
    [HideInInspector] public float direction;

    private void Update()
    {
        if (moveY != 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + moveY * Time.deltaTime * 20, transform.rotation.eulerAngles.z));
            rotModified?.Invoke(this);
        }
    }

    public void RotateReflector(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveY = context.ReadValue<Vector2>().y * direction;
        }
        if(context.canceled)
        {
            moveY = 0;
        }
    }

    public void Reset()
    {
        moveY = 0;
        direction = 0;
    }
}
