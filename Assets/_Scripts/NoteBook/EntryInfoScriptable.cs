using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_EntryInfo", menuName = "NoteBook/EntryInfoScriptable")]
public class EntryInfoScriptable : ScriptableObject
{
    [TextArea(0,15)]
    public string entryDescription;
    public Sprite entryIllustration;
    public bool hasBeenStudied;

    public bool HasBeenStudied
    {
        get { return hasBeenStudied; }
        set { hasBeenStudied = value; }
    }
}
