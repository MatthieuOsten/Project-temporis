using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNoteBook : MonoBehaviour
{
    [SerializeField] GameObject _noteBook;
    [SerializeField] NoteBookDrawer _noteBookDrawer;
    [SerializeField] EngravingInventoryScriptable _inventory;
    public void SetNoteBook()
    {
        _noteBook.SetActive(!_noteBook.activeInHierarchy);
        if (_inventory._engravingInventory != null)
            _noteBookDrawer.ShowInventory();
    }
}
