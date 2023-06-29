using UnityEngine;
using System.Collections;

public class LakeDamEvent : MonoBehaviour
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _clip;
    private PlayAudio _damAudio;

    private void Start()
    {
        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    public void LeverCorrectPosition()
    {
        _animator.SetInteger("Level", 1);
        _damAudio.PlayClipAtPoint(_source, _clip);
        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverLowPosition()
    {
        if(_lakeState.IsFlooded)
        {
            _animator.SetInteger("Level", 1);
            _damAudio.PlayClipAtPoint(_source, _clip);
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 2);
            _damAudio.PlayClipAtPoint(_source, _clip);
        }
        else
        {
            _animator.SetInteger("Level", 2);
            _damAudio.PlayClipAtPoint(_source, _clip);
        }

        _lakeState.IsLow = true;
        _lakeState.CheckState();
    }

    public void LeverBackToCorrectPosition()
    {

        _lakeState.IsCorrect = true;
        _animator.SetInteger("Level", 3);
        _damAudio.PlayClipAtPoint(_source, _clip);
        _lakeState.CheckState();
    }

    public void LeverHighPosition()
    {
        if (_lakeState.IsLow)
        {
            _animator.SetInteger("Level", 3);
            _damAudio.PlayClipAtPoint(_source, _clip);
            StartCoroutine(WaitEndAnimation());
            _animator.SetInteger("Level", 4);
            _damAudio.PlayClipAtPoint(_source, _clip);
        }
        else
        {
            _animator.SetInteger("Level", 4);
            _damAudio.PlayClipAtPoint(_source, _clip);
        }

        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    IEnumerator WaitEndAnimation()
    { yield return new WaitForSeconds(2); }
}
