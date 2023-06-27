using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameUI _gameUI;
    [SerializeField] EntriesListScriptable _entriesList;

    [Header("RAYCAST UTILITIES")]
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _interactLayers;
    [SerializeField] private LayerMask _itemReceiverLayer;
    [SerializeField] private Transform _pickUpPoint;

    private EntryHolder _entryHolderInfo;
    private PickableItem _itemInfo;
    private PickableItem _currentHeldItemInfo;
    private ItemReceiver _itemReceiverInfo;
    private TornPageHolder _tornPageInfo;

    bool canDetect = true;
    bool isHoldingItem = false;

    [SerializeField] GameObject arm;

    [Header("PLAYER UTILITIES")]
    [SerializeField] private PlayerMovement _playerMov;
    [SerializeField] private HeadBobbing _headBobbing;
    [SerializeField] private PlayerCamera _playerCam;

    [Header("INVENTORY")]
    [SerializeField] private InventoryScriptable _inventoryScriptable;

    // Update is called once per frame
    void Update()
    {
        if(canDetect)
        {
            // create a ray at the center of the camera shooting outwards
            Ray ray = new Ray(CameraUtility.Camera.transform.position, CameraUtility.Camera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * _rayDistance);

            if (!isHoldingItem)
            {
                DetectInteract(ray);
            }
            else
            {
                DetectItemReceiver(ray);
            }
        }
    }

    #region RAYCAST UTILITIES
    private void Reset()
    {
        _gameUI.HideInteractText();
        InputManager.Instance.InteractStarted = null;
        InputManager.Instance.InteractCancelled = null;
    }

    private void DetectInteract(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _rayDistance, _interactLayers))
        {
            if (hitInfo.transform.gameObject.layer == 6)
            {
                if(hitInfo.collider.TryGetComponent<EntryHolder>(out _entryHolderInfo))
                {
                    _gameUI.ShowInteractText("Left click to observe");
                    InputManager.Instance.InteractStarted = WriteEntry;
                }
            }
            else if (hitInfo.transform.gameObject.layer == 11)
            {
                _itemInfo = hitInfo.transform.GetComponent<PickableItem>();
                if (!_inventoryScriptable.AlreadyContained(_itemInfo.Info))
                {
                    _gameUI.ShowInteractText("Left click to pick");
                    InputManager.Instance.InteractStarted = OnItemPicked;
                }
                else if(_itemInfo.picked)
                {
                    _gameUI.ShowInteractText("Left click to pick");
                    InputManager.Instance.InteractStarted = OnItemPickedFromReceiver;
                }
            }
            else if(hitInfo.transform.gameObject.layer == 8)
            {
                if(hitInfo.collider.TryGetComponent<TornPageHolder>(out _tornPageInfo))
                {
                    _gameUI.ShowInteractText("Lef click to pick");
                    InputManager.Instance.InteractStarted = OnTornPagePicked;
                }
            }
        }
        else
        {
            Reset();
        }
    }

    private void DetectItemReceiver(Ray ray)
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, _rayDistance, _itemReceiverLayer))
        {
            _gameUI.ShowInteractText("Left click to use");
            _itemReceiverInfo = hitInfo.transform.GetComponent<ItemReceiver>();
            InputManager.Instance.InteractStarted = UseItem;
        }
        else
        {
            _gameUI.HideInteractText();
        }
    }
    #endregion

    #region PRINT
    /*public void TranslatePrint(InputAction.CallbackContext context)
    {
        EngravingScriptable engravingScriptableToAdd = _engravingInfo.EngravingScriptable;
        _noteBook.Set(engravingScriptableToAdd);
        _engravingInfo.EngravingScriptable.HasBeenStudied = true;
        _pageInventory.SetPageInfo(engravingScriptableToAdd); //ajoute la page à la liste
        InputManager.Instance.InteractStarted -= TranslatePrint;
    }*/

    public void WriteEntry(InputAction.CallbackContext context)
    {
        Debug.Log("Pourquoi?");
        Reset();
        _entriesList.AddEntry(_entryHolderInfo.Info);
        _gameUI.HideInteractText();
    }
    #endregion

    #region ITEM
    void OnItemPicked(InputAction.CallbackContext context)
    {
        Reset();
        _inventoryScriptable.AddItem(_itemInfo);
        _itemInfo.transform.parent = _pickUpPoint;
        _itemInfo.transform.position = _pickUpPoint.position;
        _itemInfo.itemPickedUp += OnItemPickedUp;
        _itemInfo.itemStored += OnItemStored;
        _itemInfo.itemPicked?.Invoke();
    }
    void OnItemPickedFromReceiver(InputAction.CallbackContext context)
    {
        Reset();
        _inventoryScriptable.AddItem(_itemInfo);
        _itemInfo.transform.parent = _pickUpPoint;
        _itemInfo.transform.position = _pickUpPoint.position;
        _itemInfo.itemPickedFromReceiver?.Invoke(_itemInfo);
    }
    public void OnItemPickedUp(PickableItem item)
    {
        isHoldingItem = true;
        _currentHeldItemInfo = item;
        _gameUI.ShowItem(item.Info.View);
    }
    public void OnItemStored()
    {
        isHoldingItem = false;
        _currentHeldItemInfo = null;
        _gameUI.HideItem();
    }
    void UseItem(InputAction.CallbackContext context)
    {
        ItemInfoScriptable itemInfo = _currentHeldItemInfo.Info;
        if (_currentHeldItemInfo.Info.Id == _itemReceiverInfo.linckedItemId)
        {
            _itemReceiverInfo.RightItemReceived?.Invoke(_currentHeldItemInfo);
        }
        else
        {
            _itemReceiverInfo.WrongItemReceived?.Invoke(_currentHeldItemInfo);
        }
        _inventoryScriptable.RemoveItem(itemInfo);
        _currentHeldItemInfo.itemUsed.Invoke(_itemReceiverInfo.itemPosition);
        _itemReceiverInfo.linckedItemInfo = itemInfo;
        isHoldingItem = false;
        _gameUI.HideItem();
        Reset();
    }
    #endregion

    #region TORNED PAGE
    void OnTornPagePicked(InputAction.CallbackContext context)
    {
        Reset();
        _entriesList.AddTornedEntries(_tornPageInfo.FrontEntryInfo, _tornPageInfo.BackEntryInfo);
        Destroy(_tornPageInfo.gameObject);
    }
    #endregion
}
