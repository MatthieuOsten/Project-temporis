using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrb : MonoBehaviour
{
    [SerializeField] GameObject effects;

    public void ActivateOrb()
    {
        effects.SetActive(true);
    }
}
