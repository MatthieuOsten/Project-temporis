using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactive : MonoBehaviour
{
    private int nbrBeenUsed = 0;

    private InputAction.CallbackContext actualContext;

    public virtual void StartedUse(InputAction.CallbackContext context) {}

    public virtual void CancelledUse(InputAction.CallbackContext context) {}

    public override string ToString()
    {
        return "Used " + nbrBeenUsed.ToString() + " | context -> " + actualContext.ToString();
    }
}