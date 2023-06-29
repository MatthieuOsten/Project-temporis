using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] AudioPlayManager _audioManager;
    [SerializeField] AudioClip _clip;

    public void PlayClip()
    {
        _audioManager.SetAudioClipOneShot(_clip);
    }

    public void StopPlay()
    {
        _audioManager.Source.Stop();
    }
}
