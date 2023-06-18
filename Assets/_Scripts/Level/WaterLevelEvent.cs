using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevelEvent : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void WaterLevel1()
    {
        _animator.SetInteger("Level", 1);
    }

    public void WaterLevel2()
    {
        _animator.SetInteger("Level", 2);
    }
}
