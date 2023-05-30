using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemInfo", menuName = "Puzzle/ItemInfoScriptable")]
public class ItemInfoScriptable : ScriptableObject
{
    [SerializeField] int _id;
    public int Id { get { return _id; } private set { _id = value; } }
    [SerializeField] Sprite _view;
    public Sprite View { get { return _view; } private set { _view = value; } }
}