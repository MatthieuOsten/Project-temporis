using UnityEngine;
using System.Collections;

public class LakeDamEvent : NoteBookEditableElement
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;
    [SerializeField] PlayAudio _damAudio;

    protected override void Start()
    {
        base.Start();
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

    protected override void OnIllustrationEdited(int index)
    {
        base.OnIllustrationEdited(index);
        switch(index)
        {
            case 0:
                LeverHighPosition();
                break;
            case 1:
                if(_lakeState.IsLow)
                {
                    LeverBackToCorrectPosition();
                }
                else if(_lakeState.IsFlooded)
                {
                    LeverCorrectPosition();
                }
                break;
            case 2:
                LeverLowPosition();
                break;
        }
    }
}