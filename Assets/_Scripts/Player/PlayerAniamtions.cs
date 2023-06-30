using UnityEngine;

public class PlayerAniamtions : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void SetBoolAnim(string name, bool state)
    {
        _animator.SetBool(name, state);
    }
}
