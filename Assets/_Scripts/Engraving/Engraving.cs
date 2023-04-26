using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engraving : MonoBehaviour
{
    //Classe pour r�cup�rer le Scriptable de la gravure

    [SerializeField] EngravingScriptable _engravingScriptable;

    public EngravingScriptable EngravingScriptable
    {
        get { return _engravingScriptable; }
    }

    private void Start()
    {
        EngravingScriptable.hasBeenStudied = false;
    }
}
