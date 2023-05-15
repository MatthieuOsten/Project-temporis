using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Interactive : MonoBehaviour
{
    [SerializeField] protected int _nbrBeenUsed = 0;
    [SerializeField] protected bool _isUsable = false;
    [SerializeField] private InputAction.CallbackContext actualContext;

    public bool IsUsable { get { return _isUsable; } }

    public virtual void StartedUse(InputAction.CallbackContext context) {
        actualContext = context;
    }

    public virtual void CancelledUse(InputAction.CallbackContext context) {
        actualContext = context;
    }

    public override string ToString()
    {
        return "Used " + _nbrBeenUsed.ToString() + " | context -> " + actualContext.ToString();
    }
}