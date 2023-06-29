using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNight : MonoBehaviour
{
    [SerializeField] GameObject[] _particulesObjects;
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    [SerializeField] SetVisibleConstellations _constellations;
    [SerializeField] TimerBoucle _boucle;
    [SerializeField] EndOfDayAudio _endAudio;
    [SerializeField] private PlayerLocation _playerLocation;

    private void Update()
    {
        if (_boucle.CurrentTimer >= 435)
        {
            _ambiantMusic.SwitchPlayerLocationState(_playerLocation);
            _constellations.VisibleConstellations();
            for (int i = 0; i < _particulesObjects.Length; i++)
            {
                _particulesObjects[i].SetActive(!_particulesObjects[i].activeInHierarchy);
            }
        }

        if (_boucle.CurrentTimer >= 520)
            _endAudio.SetEndOfDayAudio();
    }
}
