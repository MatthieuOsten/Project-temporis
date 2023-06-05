using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_EntriesListScriptable", menuName = "NoteBook/EntriesListScriptable")]
public class EntriesListScriptable : ScriptableObject
{
    List<EngravingScriptable> _entriesList;

    public Action<EngravingScriptable, int> EntryAdded;

    public void AddPage(EngravingScriptable entryToAdd)
    {
        _entriesList.Add(entryToAdd);
        EntryAdded?.Invoke(entryToAdd, _entriesList.Count-1);
    }
    public EngravingScriptable GetEntry(int index)
    {
        return _entriesList[index];
    }

    public void Clear()
    {
        _entriesList.Clear();
    }
}
