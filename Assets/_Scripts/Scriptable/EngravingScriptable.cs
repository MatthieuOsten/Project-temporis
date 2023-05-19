using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngravingData", menuName = "Engraving/EngravingScriptable")]
public class EngravingScriptable : ScriptableObject
{
    public string engravingTranslate;
    public Sprite engravingSprite;
    public bool hasBeenStudied;

    public bool HasBeenStudied
    {
        get { return hasBeenStudied; }
        set { hasBeenStudied = value; }
    }
}
