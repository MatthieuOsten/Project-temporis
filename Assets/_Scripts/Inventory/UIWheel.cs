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
    [SerializeField] GameObject point;
    [SerializeField] GameObject _noItemText;

    private void Start()
    {
        _currentWheelRadius = 0;
        inventory.Clear();
        _itemButtons = new List<GameObject>();
        inventory.itemAdded += AddItem;
        inventory.itemRemoved += RemoveItem;
        InputManager.Instance.OpenInventoryStarted += OnOpenInventoryStarted;
        InputManager.Instance.CloseInventoryStarted += OnCloseInventoryStarted;
    }

    public void AddItem(PickableItem item, bool alreadyContained)
    {
        GameObject currentButton = Instantiate(itemButtonPrefab, wheel.transform);
        _itemButtons.Add(currentButton);
        currentButton.GetComponent<ItemButton>().SetButton(item);
        if(!alreadyContained )
        {
            item.itemPickedUp += StoreAll;
        }
        if (_itemButtons.Count > 1)
        {
            _currentWheelRadius = wheelRadius;
        }
        UpdateWheel();
    }

    public void OnOpenInventoryStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Open");
        InputManager.Instance.SwitchCurrentActionMap();
        wheel.SetActive(!wheel.activeInHierarchy);
        point.SetActive(!point.activeInHierarchy);
        if (_itemButtons.Count == 0)
        {
            _noItemText.SetActive(true);
        }
        GameUI.Instance.ShowHandCursor();
    }

    public void OnCloseInventoryStarted(InputAction.CallbackContext context)
    {
        InputManager.Instance.SwitchCurrentActionMap();
        wheel.SetActive(!wheel.activeInHierarchy);
        point.SetActive(!point.activeInHierarchy);
        _noItemText.SetActive(false);
        GameUI.Instance.HideCursor();
    }

    public void RemoveItem(int index)
    {
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

    public void StoreAll(PickableItem item)
    {
        for (int i = 0; i < _itemButtons.Count; i++)
        {
            PickableItem currentItem = _itemButtons[i].GetComponent<ItemButton>().linckedItem;
            if (currentItem != item)
            {
                currentItem.itemStored?.Invoke();
            }
        }
    }
}