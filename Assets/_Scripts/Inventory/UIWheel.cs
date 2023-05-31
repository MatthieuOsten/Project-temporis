using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIWheel : MonoBehaviour
{
    [Header("INVENTORY")]
    [SerializeField] InventoryScriptable inventory;

    [Header("WHEEL")]
    [SerializeField] GameObject wheel;
    float _currentWheelRadius;
    [SerializeField] float wheelRadius;
    [SerializeField] GameObject itemButtonPrefab;
    [SerializeField] List<GameObject> _itemButtons;

    private void Start()
    {
        _currentWheelRadius = 0;
        inventory.Clear();
        _itemButtons = new List<GameObject>();
        inventory.itemAdded += AddItem;
        inventory.itemRemoved += RemoveItem;
        InputManager.Instance.InventoryStarted += OnInventoryStarted;
    }

    public void AddItem(ItemInfoScriptable itemInfo)
    {
        GameObject currentButton = Instantiate(itemButtonPrefab, wheel.transform);
        _itemButtons.Add(currentButton);
        currentButton.GetComponent<ItemButton>().SetButton(itemInfo);
        if(_itemButtons.Count > 1)
        {
            _currentWheelRadius = wheelRadius;
        }
        UpdateWheel();
    }

    public void OnInventoryStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Inventory");
        wheel.SetActive(!wheel.activeInHierarchy);
    }

    public void RemoveItem(int index)
    {
        Debug.Log(index);
        GameObject buttonToRemove = _itemButtons[index];
        _itemButtons.Remove(buttonToRemove);
        Destroy(buttonToRemove);
        if(_itemButtons.Count <= 1)
        {
            _currentWheelRadius = 0;
        }
        UpdateWheel();
    }

    void UpdateWheel()
    {
        float length = 2 * Mathf.PI / _itemButtons.Count;
        for (int i = 0; i < _itemButtons.Count; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Mathf.Cos(length * i), transform.position.y + Mathf.Sin(length * i), transform.position.z);
            Vector3 dir = (transform.position - pos).normalized;
            pos -= dir * _currentWheelRadius;
            _itemButtons[i].transform.position = pos;
        }
    }
}