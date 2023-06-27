using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeLevelEvent : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void BridgeCorrectLevel()
    {
        _animator.SetInteger("Level", 1);
    }

    public void BridgeLowLevel()
    {
        _animator.SetInteger("Level", 2);
    }

    public void BridgeBackToCorrectLevel()
    {
        _animator.SetInteger("Level", 3);
    }

    public void BridgeFloodedLevel()
    {
        _animator.SetInteger("Level", 4);
    }
}
