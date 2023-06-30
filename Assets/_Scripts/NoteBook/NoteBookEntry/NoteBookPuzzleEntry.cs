using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteBookPuzzleEntry : NoteBookEntry
{
    [SerializeField] TextMeshProUGUI _entryDescription;
    public override void SetEntry(EntryScriptable info)
    {
        Debug.Log(info);
        PuzzleEntryScriptable currentInfo = info as PuzzleEntryScriptable;
        _entryDescription.text = currentInfo.DescriptionStates[0];
    }
}
