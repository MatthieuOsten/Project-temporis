using UnityEngine;

public class PlayAudio : MonoBehaviour
{

    public void PlayClip(AudioSource source, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, source.gameObject.transform.position);
    }

    public void StopPlay(AudioSource source)
    {
        source.Stop();
    }
}
