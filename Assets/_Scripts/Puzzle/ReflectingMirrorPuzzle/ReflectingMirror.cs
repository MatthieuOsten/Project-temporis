using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirror : MonoBehaviour
{
    public ReflectingMirrorPuzzle reflectingMirrorPuzzle;
    public Action<ReflectingMirror> rotModified;

    [SerializeField] AudioClip _rotatingClip;
    [SerializeField] AudioSource _source;
    private PlayAudio _audio;

    private SpawnParticule _particule;
    [SerializeField] private ParticleSystem _particuleToSpawn;

    private void Start()
    {
        _particuleToSpawn.Stop();
    }

    public IEnumerator RotateToward(float rotY)
    {
        reflectingMirrorPuzzle.LockAllMirrorsButtons();
        Quaternion rot = Quaternion.Euler(0, rotY, 0);
        while (transform.rotation != rot)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 100f * Time.deltaTime);
            _particule.PlayParticule(_particuleToSpawn);
            _audio.PlayClipAtPoint(_source, _rotatingClip);
            rotModified?.Invoke(this);
            yield return null;
        }
        yield return StartCoroutine(CheckRot(rot));
        rotModified?.Invoke(this);
        reflectingMirrorPuzzle.UnlockAllMirrorsButtons();
        _particule.StopParticule(_particuleToSpawn);
        _audio.StopPlay(_source, _rotatingClip);
    }

    IEnumerator CheckRot(Quaternion rot)
    {
        transform.rotation = rot;
        yield return null;
    }
}
