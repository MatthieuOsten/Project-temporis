using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightIntensity : MonoBehaviour
{
    [SerializeField] Light[] _lights;

    private void Start()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = 1;
        }
    }

    public void SetLight(int intensity)
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = intensity;
        }
    }
}
