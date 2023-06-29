using UnityEngine;
using System.Collections;

public class LakeDamEvent : MonoBehaviour
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;
    [SerializeField] PlayAudio _damAudio;

    private void Start()
    {
        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    public void LeverCorrectPosition()
    {
        _animator.SetInteger("Level", 1);
        _damAudio.PlayClip();
        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverLowPosition()
    {
        if(_lakeState.IsFlooded)
        {
            _animator.SetInteger("Level", 1);
            _damAudio.PlayClip();
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 2);
            _damAudio.PlayClip();
        }
        else
        {
            _animator.SetInteger("Level", 2);
            _damAudio.PlayClip();
        }

        _lakeState.IsLow = true;
        _lakeState.CheckState();
    }

    public void LeverBackToCorrectPosition()
    {

        _lakeState.IsCorrect = true;
        _animator.SetInteger("Level", 3);
        _damAudio.PlayClip();
        _lakeState.CheckState();
    }

    public void LeverHighPosition()
    {
        if (_lakeState.IsLow)
        {
            _animator.SetInteger("Level", 3);
            _damAudio.PlayClip();
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 4);
            _damAudio.PlayClip();
        }
        else
        {
            _animator.SetInteger("Level", 4);
            _damAudio.PlayClip();
        }

        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    IEnumerator WaitEndAnimation()
    { yield return new WaitForSeconds(2); }
}
