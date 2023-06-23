using UnityEngine;
using System;
using System.Collections;

public class LeverEvent : MonoBehaviour
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;

    private void Start()
    {
        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
        LeverLowPosition();
    }

    public void LeverCorrectPosition()
    {
        _animator.SetInteger("Level", 1);

        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverLowPosition()
    {
        if(_lakeState.IsFlooded)
        {
            _animator.SetInteger("Level", 1);
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 2);
        }
        else
        {
            _animator.SetInteger("Level", 2);
        }

        _lakeState.IsLow = true;
        _lakeState.CheckState();
    }

    public void LeverBackToCorrectPosition()
    {

        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverHighPosition()
    {
        if (_lakeState.IsLow)
        {
            _animator.SetInteger("Level", 3);
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 4);
        }
        else
        {
            _animator.SetInteger("Level", 4);
        }

        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    IEnumerator WaitEndAnimation()
    { yield return new WaitForSeconds(2); }
}
