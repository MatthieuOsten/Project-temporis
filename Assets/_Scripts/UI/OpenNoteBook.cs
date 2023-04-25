using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNoteBook : MonoBehaviour
{
    [SerializeField] GameObject _noteBook;

    /// <summary>
    /// Set the visibility of the NoteBook gameObject
    /// </summary>
    public void SetNoteBook()
    {
        _noteBook.SetActive(!_noteBook.activeInHierarchy);
    }
}
