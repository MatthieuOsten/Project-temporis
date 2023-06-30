using UnityEngine;
using System.Collections;

public class LakeDamEvent : NoteBookEditableElement
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;

    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _clip;

    protected override void Start()
    {
        base.Start();
        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    private void Update()
    {
        Debug.Log(_animator.GetInteger("Level"));
    }

    public void LeverCorrectPosition()
    {
        _lakeState.IsFlooded = false;
        _lakeState.IsCorrect = true;
        _animator.SetInteger("Level", 1);
        PlayAudio.PlayClip(_source, _clip);
        _lakeState.CheckState();
    }

    public void LeverLowPosition()
    {

        if (_lakeState.IsFlooded)
        {
            _lakeState.IsFlooded = false;
            StartCoroutine(WaitEndAnimation(1));
            //_animator.SetInteger("Level", 2);
            PlayAudio.PlayClip(_source, _clip);
        }
        else
        {
            _lakeState.IsCorrect = false;
            _animator.SetInteger("Level", 2);
            PlayAudio.PlayClip(_source, _clip);
        }

        _lakeState.IsLow = true;
        _lakeState.CheckState();
    }

    public void LeverBackToCorrectPosition()
    {
        _lakeState.IsLow = false;
        _animator.SetInteger("Level", 3);
        PlayAudio.PlayClip(_source, _clip);
        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverHighPosition()
    {

        if (_lakeState.IsLow)
        {
            _lakeState.IsLow = false;
            StartCoroutine(WaitEndAnimation(3));
            PlayAudio.PlayClip(_source, _clip);
        }
        else
        {
            _lakeState.IsCorrect = false;
            _animator.SetInteger("Level", 4);
            PlayAudio.PlayClip(_source, _clip);
        }

        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    IEnumerator WaitEndAnimation(int level)
    {
        _animator.SetInteger("Level", level);
        PlayAudio.PlayClip(_source, _clip);
        yield return new WaitForSeconds(2);
        _animator.SetInteger("Level", level + 1);
    }

    protected override void OnIllustrationEdited(int index)
    {
        base.OnIllustrationEdited(index);
        switch (index)
        {
            case 0: // flooded
                LeverHighPosition();
                break;
            case 1: // correct
                if (_lakeState.IsLow)
                {
                    LeverBackToCorrectPosition();
                }
                else if (_lakeState.IsFlooded)
                {
                    LeverCorrectPosition();
                }
                break;
            case 2: // low
                LeverLowPosition();
                break;
        }
    }
}