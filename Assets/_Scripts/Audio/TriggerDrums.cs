using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDrums : MonoBehaviour
{
    [SerializeField] AudioSource _drumsSource, _musicSource;
    [SerializeField] AudioClip _clip;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _drumsSource.Pause();
            Destroy(gameObject);
            _musicSource.clip = _clip;
            _musicSource.Play();
        }
    }
}
