using System.Collections;
using UnityEngine;

public class SwitchAmbiantMusic : MonoBehaviour
{
    [SerializeField] float _transitionTime;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _audioClips;
    private PlayerLocation _playerLocation;
    private float _timer;

    private void Start()
    {
        SwitchPlayerLocationState(PlayerLocation.InForest);
    }

    public void SwitchPlayerLocationState(PlayerLocation playerLocation)
    {
        if (_playerLocation == playerLocation)
        {
            return;
        }
        else
        {
            _playerLocation = playerLocation;
            StartCoroutine(SwitchAmbiant());
        }
    }

    private void SwitchClip()
    {
        switch (_playerLocation)
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

    private void TurnVolume(float timer, float transitionTime, float startVolume, float endVolume)
    {
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            // Calcul de l'interpolation linéaire du volume
            _audioSource.volume = Mathf.Lerp(startVolume, endVolume, timer / transitionTime);
        }
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
        PlayAudio();

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
