using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteBookStandardEntry : NoteBookEntry
{
    [SerializeField] TextMeshProUGUI[] _entryDescriptions;
    [SerializeField] Image[] _entryIllustrations;
    public override void SetEntry(EntryScriptable info)
    {
        StandardEntryScriptable currentInfo = info as StandardEntryScriptable;
        for(int i = 0; i<_entryDescriptions.Length; i++)
        {
            _entryDescriptions[i].text = currentInfo.EntryDescriptions[i];
        }
        for(int i = 0; i<_entryIllustrations.Length; i++)
        {
            _entryIllustrations[i].sprite = currentInfo.EntryIllustrations[i];
        }
    }
}
