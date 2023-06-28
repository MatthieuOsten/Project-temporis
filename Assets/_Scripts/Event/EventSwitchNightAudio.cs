using UnityEngine;

public class EventSwitchNightAudio : MonoBehaviour
{
    [SerializeField] TimerBoucle _boucle;
    [SerializeField] SwitchAmbiantMusic _ambiantMusic;
    private PlayerLocation _playerLocation = PlayerLocation.InForest;

    private void Update()
    {
        if (_boucle.CurrentTimer >= 435)
        {
            _playerLocation = PlayerLocation.InNight;
            _ambiantMusic.CheckPlayerLocation();
        }
    }
}
