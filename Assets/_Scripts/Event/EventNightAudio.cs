using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNightAudio : MonoBehaviour
{
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    [SerializeField] TimerBoucle _boucle;
    [SerializeField] EndOfDayAudio _endAudio;
    [SerializeField] private PlayerLocation _playerLocation;

    private void Update()
    {
        if(_boucle.CurrentTimer >= 435)
            _ambiantMusic.SwitchPlayerLocationState(_playerLocation);

        if (_boucle.CurrentTimer >= 520)
            _endAudio.SetEndOfDayAudio();
    }
}
