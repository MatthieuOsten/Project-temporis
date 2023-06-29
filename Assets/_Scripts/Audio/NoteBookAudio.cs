using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookAudio : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] _turnPageClip;
    [SerializeField] AudioClip[] _writingClip;
    [SerializeField] AudioClip _openBook;
    [SerializeField] AudioClip _eraseClip;
    AudioClip previousClip;

    public void OpenNoteBookSound()
    {
        source.PlayOneShot(_openBook);
    }

    public void EraseClip()
    {
        source.PlayOneShot(_eraseClip);
    }

    public void TurnPageClip()
    {
        source.PlayOneShot(GetClip(_turnPageClip));
    }

    public void WritingClip()
    {
        source.PlayOneShot(GetClip(_writingClip));
    }

    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }

}
