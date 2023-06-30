using UnityEngine;

public static class PlayAudio
{
    public static void PlayClip(AudioSource source, AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, source.gameObject.transform.position);
    }

    public static void StopPlay(AudioSource source)
    {
        source.Stop();
    }
}
