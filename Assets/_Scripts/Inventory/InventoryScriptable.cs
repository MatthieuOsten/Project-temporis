using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_InventoryScriptable", menuName = "Inventory/Inventory Scriptable")]
public class InventoryScriptable : ScriptableObject
{
    [SerializeField] List<ItemInfoScriptable> _currentItemsList;
    List<ItemInfoScriptable> _itemsList;
    public Action<ItemInfoScriptable> itemAdded;
    public Action<int> itemRemoved;

    public void AddItem(ItemInfoScriptable itemInfo)
    {
        _currentItemsList.Add(itemInfo);
        itemAdded?.Invoke(itemInfo);
        itemInfo.itemPicked?.Invoke();
        itemInfo.itemPickedUp += StoreAll;
    }
    public void RemoveItem(ItemInfoScriptable itemInfo)
    {
        itemRemoved?.Invoke(_currentItemsList.IndexOf(itemInfo));
        _currentItemsList.Remove(itemInfo);
    }
    public bool CurrentlyContained(ItemInfoScriptable itemInfo)
    {
        return _currentItemsList.Contains(itemInfo);
    }
    public bool AlreadyContained(ItemInfoScriptable itemInfo)
    {
        return _itemsList.Contains(itemInfo);
    }
    public void StoreAll(ItemInfoScriptable itemInfo)
    {
        for(int i = 0; i< _currentItemsList.Count; i++)
        {
            if(_currentItemsList[i] != itemInfo)
            {
                _currentItemsList[i].itemStored?.Invoke();
            }
        }
    }
    public void Clear()
    {
        _currentItemsList.Clear();
        _itemsList.Clear();
        itemAdded = null;
        itemRemoved = null;
    }
}
