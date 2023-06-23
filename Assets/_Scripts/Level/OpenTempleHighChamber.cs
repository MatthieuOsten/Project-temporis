using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTempleHighChamber : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void TempleHighChamber()
    {
        _animator.SetBool("MirorCompleted", true);
    }
}
