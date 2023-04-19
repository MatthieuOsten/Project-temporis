using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookDrawer : MonoBehaviour
{
    [SerializeField] GameObject _noteBookContent;
    [SerializeField] PagesList _pages;
    [SerializeField] EngravingUI _noteBookPage;
    [SerializeField] EngravingInventoryScriptable _inventory;

    public void ShowInventory()
    {
        foreach (Transform item in _noteBookContent.transform)
        {
            Destroy(item.gameObject);
        }

        foreach (EngravingScriptable item in _inventory._engravingInventory)
        {
            EngravingUI obj = Instantiate(_noteBookPage, _noteBookContent.transform);

            obj.Set(item);
            _pages.GetPages();
        }
    }
}
