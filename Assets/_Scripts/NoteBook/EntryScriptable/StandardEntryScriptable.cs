using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_StandardEntry", menuName = "NoteBook/StandardEntryScriptable")]
public class StandardEntryScriptable : EntryScriptable
{
    [SerializeField] string[] _entryDescriptions;
    public string[] EntryDescriptions { get { return _entryDescriptions; } }

    [SerializeField] Sprite[] _entryIllustrations;
    public Sprite[] EntryIllustrations { get {  return _entryIllustrations; } }
}
