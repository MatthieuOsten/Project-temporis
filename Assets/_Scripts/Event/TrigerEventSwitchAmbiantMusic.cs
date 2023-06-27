using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigerEventSwitchAmbiantMusic : MonoBehaviour
{
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _ambiantMusic.SwitchPlayerLocationState();
        }
    }
}
