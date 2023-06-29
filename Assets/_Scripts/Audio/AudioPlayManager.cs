using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayManager : MonoBehaviour
{
    [SerializeField] AudioSource _source;

    public AudioSource Source
    { get { return _source; } }

    public void SetAudioClipOneShot(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
