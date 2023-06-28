using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField] CheckTerrainTexture _terrainTexture;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] stoneClips;
    [SerializeField] AudioClip[] dirtClips;
    [SerializeField] AudioClip[] sandClips;
    [SerializeField] AudioClip[] grassClips;
    AudioClip previousClip;
    public void PlayFootstep()
    {
        _terrainTexture.GetTerrainTexture();
        if (_terrainTexture.TextureValues[0] > 0)
        {
            source.PlayOneShot(GetClip(stoneClips), _terrainTexture.TextureValues[0]);
        }
        if (_terrainTexture.TextureValues[1] > 0)
        {
            source.PlayOneShot(GetClip(dirtClips), _terrainTexture.TextureValues[1]);
        }
        if (_terrainTexture.TextureValues[2] > 0)
        {
            source.PlayOneShot(GetClip(dirtClips), _terrainTexture.TextureValues[2]);
        }
        if (_terrainTexture.TextureValues[3] > 0)
        {
            source.PlayOneShot(GetClip(dirtClips), _terrainTexture.TextureValues[3]);
        }
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
