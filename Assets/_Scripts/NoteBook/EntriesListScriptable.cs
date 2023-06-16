using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "new_EntriesListScriptable", menuName = "NoteBook/EntriesListScriptable")]
public class EntriesListScriptable : ScriptableObject
{
    [SerializeField] EntryInfoScriptable[] _entriesList;

    public Action<EntryInfoScriptable, int> entryAdded;
    public Action<EntryInfoScriptable, EntryInfoScriptable> tornedEntriesAdded;

    public void AddEntry(EntryInfoScriptable entryToAdd)
    {
        _entriesList[entryToAdd.entryIndex] = entryToAdd;
        entryAdded?.Invoke(entryToAdd, entryToAdd.entryIndex);
    }
    public void AddTornedEntries(EntryInfoScriptable frontEntry, EntryInfoScriptable backEntry)
    {
        _entriesList[frontEntry.entryIndex] = frontEntry;
        tornedEntriesAdded?.Invoke(frontEntry, backEntry);
        _entriesList[backEntry.entryIndex] = backEntry;
    }
    public EntryInfoScriptable GetEntry(int index)
    {
        return _entriesList[index];
    }

    public void Clear()
    {
        _entriesList = null;
    }
}
