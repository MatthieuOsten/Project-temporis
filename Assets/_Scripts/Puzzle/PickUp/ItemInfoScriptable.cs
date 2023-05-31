using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new_ItemInfoScriptable", menuName = "Inventory/ItemInfo Scriptable")]
public class ItemInfoScriptable : ScriptableObject
{
    [SerializeField] int _id;
    public int Id { get { return _id; } private set { _id = value; } }
    [SerializeField] Sprite _view;
    public Sprite View { get { return _view; } private set { _view = value; } }
    [SerializeField] string _name;
    public string Name { get { return _name; } private set { _name = value; } }

    public Action itemPicked, itemStored;
    public Action<ItemInfoScriptable> itemPickedUp;
    public Action<Transform> itemUsed;
}