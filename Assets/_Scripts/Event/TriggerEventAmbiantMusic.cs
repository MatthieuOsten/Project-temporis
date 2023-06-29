using UnityEngine;

public class TriggerEventAmbiantMusic : MonoBehaviour
{
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    [SerializeField] private PlayerLocation _playerLocation;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _ambiantMusic.SwitchPlayerLocationState(_playerLocation);
        }
    }
}
