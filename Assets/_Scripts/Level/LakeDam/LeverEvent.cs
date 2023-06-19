using UnityEngine;

public class LeverEvent : MonoBehaviour
{
    [SerializeField] LakeState _lakeState;
    [SerializeField] Animator _animator;

    private void Start()
    {
        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }

    public void LeverCorrectPosition()
    {
        _animator.SetInteger("Level", 1);

        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverLowPosition()
    {
        _animator.SetInteger("Level", 2);

        _lakeState.IsLow = true;
        _lakeState.CheckState();
    }

    public void LeverBackToCorrectPosition()
    {

        _lakeState.IsCorrect = true;
        _lakeState.CheckState();
    }

    public void LeverHighPosition()
    {
        if (_lakeState.IsLow)
        {
            _animator.SetInteger("Level", 3);

            if (!_animator.IsInTransition(3)) ;
            {
                _animator.SetInteger("Level", 4);

            }
        }
        else
        {
            _animator.SetInteger("Level", 4);
        }

        _lakeState.IsFlooded = true;
        _lakeState.CheckState();
    }
}
