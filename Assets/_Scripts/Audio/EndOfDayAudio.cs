using UnityEngine;

public class EndOfDayAudio : MonoBehaviour
{
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _clip;

    public void SetEndOfDayAudio()
    {
        _source.clip = _clip;
        _source.Play();
    }
}
