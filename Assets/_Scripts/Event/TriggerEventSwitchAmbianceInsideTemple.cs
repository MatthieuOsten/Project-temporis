using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventSwitchAmbianceInsideTemple : MonoBehaviour
{
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    private PlayerLocation _playerLocation = PlayerLocation.InForest;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (_playerLocation == PlayerLocation.InForest)
            {
                _playerLocation = PlayerLocation.InsideTemple;
                _ambiantMusic.CheckPlayerLocation();
            }
            else
            {
                _playerLocation = PlayerLocation.InForest;
                _ambiantMusic.CheckPlayerLocation();
            }
        }
    }
}
