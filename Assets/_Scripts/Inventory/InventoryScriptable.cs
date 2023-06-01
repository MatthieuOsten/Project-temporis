using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_InventoryScriptable", menuName = "Inventory/Inventory Scriptable")]
public class InventoryScriptable : ScriptableObject
{
    [SerializeField] List<ItemInfoScriptable> _currentItemsList;
    List<ItemInfoScriptable> _itemsList;
    public Action<PickableItem, bool> itemAdded;
    public Action<int> itemRemoved;

    public void AddItem(PickableItem item)
    {
        ItemInfoScriptable itemInfo = item.Info;
        bool alreadyContained = AlreadyContained(itemInfo);
        _currentItemsList.Add(itemInfo);
        if(!alreadyContained)
        {
            _itemsList.Add(itemInfo);
        }
        itemAdded?.Invoke(item, alreadyContained);
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
    public void Clear()
    {
        _currentItemsList.Clear();
        _itemsList.Clear();
        itemAdded = null;
        itemRemoved = null;
    }
}
