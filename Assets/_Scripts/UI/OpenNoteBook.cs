using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNoteBook : MonoBehaviour
{
    [SerializeField] GameObject _noteBook;
    public void SetNoteBook()
    {
        _noteBook.SetActive(!_noteBook.activeInHierarchy);
    }
}
