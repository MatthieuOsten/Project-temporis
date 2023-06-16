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
    }
}

