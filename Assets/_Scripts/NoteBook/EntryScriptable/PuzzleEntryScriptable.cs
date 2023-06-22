using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_PuzzleEntry", menuName = "NoteBook/PuzzleEntryScriptable")]
public class PuzzleEntryScriptable : EntryScriptable
{
    [SerializeField, TextArea(0, 5)] string[] _descriptionStates;
    public string[] DescriptionStates { get { return _descriptionStates; } }
}
