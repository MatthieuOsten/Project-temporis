using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngravingInventoryData", menuName = "Engraving/EngravingInventoryScriptable")]
public class EngravingInventoryScriptable : ScriptableObject
{
    public List<EngravingScriptable> _engravingInventory;

    private void OnEnable()
    {
        if (_engravingInventory != null) //Si la liste d'item n'est pas a null
        {
            _engravingInventory.Clear(); //On la Clear
        }
        else
        {
            _engravingInventory = new List<EngravingScriptable>(); //Si la liste est nulle, on en creer une nouvelle
        }
    }

    public void AddEngravingToNoteBook(EngravingScriptable item)
    {
        _engravingInventory.Add(item);
    }
}
