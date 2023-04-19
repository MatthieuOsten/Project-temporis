using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engraving : MonoBehaviour
{
    [SerializeField] EngravingScriptable _engravingScriptable;

    public EngravingScriptable EngravingScriptable
    {
        get { return _engravingScriptable; }
    }
}
