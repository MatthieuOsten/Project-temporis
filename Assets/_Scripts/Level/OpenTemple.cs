using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTemple : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] DamSceneUI _DamUI;
    [SerializeField] GameObject[] _waterLvl1;
    [SerializeField] GameObject[] _waterLvl2;
    [SerializeField] GameObject[] _waterLvl3;
    [SerializeField] GameObject[] _waterLvl4;
    private int _damOpen;
    private float _speed;
    private float _distance = 5f;
    private Vector3 _direction = Vector3.down;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;


    private void Start()
    {
        _speed = Time.deltaTime;
        _initialPosition = transform.position;
        _targetPosition = _initialPosition + (_direction.normalized * _distance);
    }

    private void OpenTempleDoor()
    {
        if (_damOpen == 6)
        {
            //va bouger la porte
            _animator.SetBool("PuzzleCompleted", true);
            _DamUI.ShowEndUI();
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
                for (int i = 0; i < _waterLvl1.Length; i++)
                {
                    _waterLvl1[i].SetActive(!_waterLvl1[i].activeInHierarchy);
                }
                break;

            case 3:
                for (int i = 0; i < _waterLvl2.Length; i++)
                {
                    _waterLvl2[i].SetActive(!_waterLvl2[i].activeInHierarchy);
                }
                break;

            case 5:
                for (int i = 0; i < _waterLvl3.Length; i++)
                {
                    _waterLvl3[i].SetActive(!_waterLvl3[i].activeInHierarchy);
                }
                break;

            case 6:
                for (int i = 0; i < _waterLvl4.Length; i++)
                {
                    _waterLvl4[i].SetActive(!_waterLvl4[i].activeInHierarchy);
                }
                break;
        }
    }
}
