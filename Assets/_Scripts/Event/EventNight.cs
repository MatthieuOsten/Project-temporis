using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNight : MonoBehaviour
{
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    [SerializeField] SetVisibleConstellations _constellations;
    [SerializeField] TimerBoucle _boucle;
    [SerializeField] EndOfDayAudio _endAudio;
    [SerializeField] SpawnParticule _particule;
    [SerializeField] private PlayerLocation _playerLocation;

    private void Update()
    {
        if (_boucle.CurrentTimer >= 435)
        {
            _ambiantMusic.SwitchPlayerLocationState(_playerLocation);
            _constellations.VisibleConstellations();
            _particule.Particule();
        }

        if (_boucle.CurrentTimer >= 520)
            _endAudio.SetEndOfDayAudio();
    }
}
