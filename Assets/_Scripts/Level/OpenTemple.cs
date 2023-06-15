using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTemple : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _waterLvl1;
    [SerializeField] GameObject _waterLvl2;
    [SerializeField] GameObject _waterLvl3;
    [SerializeField] GameObject _waterLvl4;
    private int _damOpen;

    private void OpenTempleDoor()
    {
        if (_damOpen == 5)
        {
            //va bouger la porte
            _animator.SetBool("PuzzleCompleted", true);
        }
    }

    public void IncreaseDamOpenCunt()
    {
        _damOpen++;
        OpenTempleDoor();
        ShowWaterInTemple();
    }

    private void ShowWaterInTemple()
    {
        switch (_damOpen)
        {
            case 1:
                    _waterLvl1.SetActive(!_waterLvl1.activeInHierarchy);
                break;

            case 3:
                    _waterLvl2.SetActive(!_waterLvl2.activeInHierarchy);
                break;

            case 5:
                    _waterLvl3.SetActive(!_waterLvl3.activeInHierarchy);
                break;

            case 6:
                    _waterLvl4.SetActive(!_waterLvl4.activeInHierarchy);
                break;
        }
    }
}
