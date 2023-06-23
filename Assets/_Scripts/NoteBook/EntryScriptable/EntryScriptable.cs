using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntryScriptable : ScriptableObject
{
    [SerializeField] protected int _entryIndex;
    public int EntryIndex { get { return _entryIndex; } }

    [SerializeField] protected Sprite _entryIcon;
    public Sprite EntryIcon { get { return _entryIcon; } }
}
