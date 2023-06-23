using UnityEngine;

public class OpenLakeDam : MonoBehaviour
{
    [SerializeField] Animator _damAnimator;

    public void OpenLakeDamCorrectLevel()
    {
        _damAnimator.SetInteger("Level", 1);
    }

    public void OpenLakeDamLowLevel()
    {
        _damAnimator.SetInteger("Level", 2);
    }

    public void BackToCorrectLevel()
    {
        _damAnimator.SetInteger("Level", 3);
    }

    public void BackToFloodedLevel()
    {
        _damAnimator.SetInteger("Level", 4);
    }
}
