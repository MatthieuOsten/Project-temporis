using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLakeDam : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Animator _damAnimator;

    private void Start()
    {
        _animator.SetInteger("Level", 2);
    }

    public void OpenLakeDamLevel1()
    {
        _damAnimator.SetInteger("Level", 1);
    }

    public void OpenLakeDamLevel2()
    {
        _damAnimator.SetInteger("Level", 2);
    }
}
