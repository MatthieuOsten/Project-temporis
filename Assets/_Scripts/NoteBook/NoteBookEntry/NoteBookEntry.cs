using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBookEntry : MonoBehaviour
{
    [SerializeField] protected GameObject _entryCanvas;
    public abstract void SetEntry(EntryScriptable info);
    public virtual void ShowEntry()
    {
        _entryCanvas.SetActive(true);
    }
    public virtual void HideEntry()
    {
        _entryCanvas.SetActive(false);
    }
}
