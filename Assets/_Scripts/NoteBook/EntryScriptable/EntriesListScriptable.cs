using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new_EntriesListScriptable", menuName = "NoteBook/EntriesListScriptable")]
public class EntriesListScriptable : ScriptableObject
{
    [SerializeField] EntryScriptable[] _entriesList;

    public Action<EntryScriptable, int> entryAdded;
    public Action<EntryScriptable, EntryScriptable> tornedEntriesAdded;

    public void AddEntry(EntryScriptable entryToAdd)
    {
        _entriesList[entryToAdd.EntryIndex] = entryToAdd;
        entryAdded?.Invoke(entryToAdd, entryToAdd.EntryIndex);
    }
    public void AddTornedEntries(EntryScriptable frontEntry, EntryScriptable backEntry)
    {
        _entriesList[frontEntry.EntryIndex] = frontEntry;
        tornedEntriesAdded?.Invoke(frontEntry, backEntry);
        _entriesList[backEntry.EntryIndex] = backEntry;
    }
    public EntryScriptable GetEntry(int index)
    {
        return _entriesList[index];
    }

    public void Clear()
    {
        _entriesList = null;
    }
}
