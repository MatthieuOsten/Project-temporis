using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDrumsAudio : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    [SerializeField] Transform _source;

    private void Start()
    {
        AudioSource.PlayClipAtPoint(_clip, _source.position);
    }
}
