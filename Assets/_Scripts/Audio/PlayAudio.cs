using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{

    public void PlayClipAtPoint(AudioSource source, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, source.gameObject.transform.position); 
    }

    public void StopPlay(AudioSource source, AudioClip clip)
    {
        source.Stop();
    }
}
