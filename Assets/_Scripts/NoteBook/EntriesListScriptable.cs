using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_EntriesListScriptable", menuName = "NoteBook/EntriesListScriptable")]
public class EntriesListScriptable : ScriptableObject
{
    List<EntryInfoScriptable> _entriesList;

    public Action<EntryInfoScriptable, int> entryAdded;

    public void AddEntry(EntryInfoScriptable entryToAdd)
    {
        _entriesList.Add(entryToAdd);
        entryAdded?.Invoke(entryToAdd, _entriesList.Count-1);
    }
    public EntryInfoScriptable GetEntry(int index)
    {
        return _entriesList[index];
    }

    public void Clear()
    {
        _entriesList.Clear();
    }
}
