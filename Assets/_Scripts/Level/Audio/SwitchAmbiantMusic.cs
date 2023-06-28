using System.Collections;
using UnityEngine;

public class SwitchAmbiantMusic : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    private PlayerLocation _playerLocation;

    private void Update()
    {
        Debug.Log(_playerLocation);
    }

    public void SwitchPlayerLocationState(PlayerLocation playerLocation)
    {
        _playerLocation = playerLocation;
        CheckPlayerLocation();

    }

    private void CheckPlayerLocation()
    {
        switch(_playerLocation)
        {
            case PlayerLocation.InForest:
                _audioSource.clip = _audioClips[0];
                //Joue l'ambiance foret
                break;

            case PlayerLocation.InDesert:
                _audioSource.clip = _audioClips[1];
                //Joue l'ambiance desert
                break;

            case PlayerLocation.InsideTemple:
                _audioSource.clip = _audioClips[2];
                //Joue l'ambiance du temple
                break;

            case PlayerLocation.InNight:
                _audioSource.clip = _audioClips[3];
                //Joue l'ambiance de nuit
                break;
        }

        PlayAudio();
    }

    private void PlayAudio()
    {
        if (_playerLocation == PlayerLocation.InsideTemple)
            _audioSource.loop = false;
        else
            _audioSource.loop = true;

        _audioSource.Play();

    }
}
