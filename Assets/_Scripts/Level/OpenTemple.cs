using UnityEngine;

public class OpenTemple : MonoBehaviour
{
    [SerializeField] Animator _animator;
    private int _damOpen;

    private void Update()
    {
        Debug.Log(_damOpen);
    }
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

