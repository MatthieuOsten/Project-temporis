using System.Collections;
using UnityEngine;

public class SwitchAmbiantMusic : MonoBehaviour
{
    private enum PlayerLocation
    {
        InForest,
        InDesert,
    }

    [SerializeField] private float _transitionTime;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    private PlayerLocation _playerLocation = PlayerLocation.InForest;
    private float _timer;

    private void CheckPlayerLocation()
    {
        switch(_playerLocation)
        {
            case PlayerLocation.InForest:
                StartCoroutine(SwitchAmbiant());
                //Joue l'ambiance foret
                break;

            case PlayerLocation.InDesert:
                StartCoroutine(SwitchAmbiant());
                //Joue l'ambiance desert
                break;
        }
    }

    public void SwitchPlayerLocationState()
    {
        if(_playerLocation == PlayerLocation.InForest)
        {
            _playerLocation = PlayerLocation.InDesert;
            CheckPlayerLocation();
        }
        else
        {
            _playerLocation = PlayerLocation.InForest;
            CheckPlayerLocation();
        }

    }

    private void TurnVolume(float timer, float transitionTime, float startVolume, float endVolume)
    {
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            // Calcul de l'interpolation linéaire du volume
            _audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / transitionTime);
        }
    }

    private void SwitchClip()
    {
        if(_audioSource.clip == _audioClips[0])
        {
            _audioSource.clip = _audioClips[1];
        }
        else
            _audioSource.clip = _audioClips[0];
    }

    IEnumerator SwitchAmbiant()
    {
        // Interpolation du volume de l'audio source pour la transition en fondu
        float startVolume = _audioSource.volume;
        float targetVolume = 0.0f;

        TurnVolume(_timer, _transitionTime, startVolume, targetVolume);
        yield return null;

        // Changement de clip audio
        SwitchClip();
        _audioSource.Play();

        // Réinitialisation du timer et des valeurs pour la transition suivante
        _timer = 0.0f;
        startVolume = 0.0f;
        targetVolume = 1.0f;

        TurnVolume(_timer, _transitionTime, startVolume, targetVolume);
        yield return null;

        // Fin de la transition
        _audioSource.volume = targetVolume;
    }
}
