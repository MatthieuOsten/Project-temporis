using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMouth : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void PlayOpenMouth()
    {
        _animator.SetBool("AllPlantsPut", true);
    }
}
