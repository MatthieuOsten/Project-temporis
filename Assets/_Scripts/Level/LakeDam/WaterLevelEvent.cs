using UnityEngine;

public class WaterLevelEvent : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void WaterCorrectLevel()
    {
        _animator.SetInteger("Level", 1);
    }

    public void WaterLowLevel()
    {
        _animator.SetInteger("Level", 2);
    }

    public void WaterBackToCorrectLevel()
    {
        _animator.SetInteger("Level", 3);
    }

    public void WaterFloodedLevel()
    {
        _animator.SetInteger("Level", 4);
    }
}
