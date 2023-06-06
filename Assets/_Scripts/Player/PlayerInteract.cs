using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameUI _gameUI;
    [SerializeField] EngravingUI _noteBook;
    [SerializeField] PageList _pageInventory;
    [SerializeField] EntriesListScriptable _entriesList;

    [Header("RAYCAST UTILITIES")]
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _interactLayers;
    [SerializeField] private LayerMask _itemReceiverLayer;
    [SerializeField] private Transform _pickUpPoint;

    private Engraving _engravingInfo;
    private EntryHolder _entryHolderInfo;
    private Transform _mirrorInfo;
    private PickableItem _itemInfo;
    private PickableItem _currentHeldItemInfo;
    private Interactive obj;
    private ItemReceiver _itemReceiverInfo;

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
                /*if (hitInfo.collider.TryGetComponent<Engraving>(out _engravingInfo))
                {
                    if (!_engravingInfo.EngravingScriptable.HasBeenStudied)
                    {
                        _gameUI.ShowInteractText("Left click to interact");
                        InputManager.Instance.InteractStarted = TranslatePrint;
                    }
                }*/
                if(hitInfo.collider.TryGetComponent<EntryHolder>(out _entryHolderInfo))
                {
                    if (!_entryHolderInfo.Info.hasBeenStudied)
                    {
                        _gameUI.ShowInteractText("Left click to observe");
                        InputManager.Instance.InteractStarted = WriteEntry;
                    }
                }
            }
            else if (hitInfo.transform.gameObject.layer == 8)
            {
                _mirrorInfo = hitInfo.transform;
                _gameUI.ShowInteractText("Hold left click to grab");
                InputManager.Instance.InteractStarted = GrabMirror;
                InputManager.Instance.InteractCancelled = LetOffMirror;
            }
            else if(hitInfo.transform.gameObject.layer == 9)
            {
                if(hitInfo.collider.TryGetComponent<Interactive>(out obj))
                {
                    _gameUI.ShowInteractText("Left click to interact");

                    InputManager.Instance.InteractStarted = obj.StartedUse;
                    InputManager.Instance.InteractCancelled = obj.CancelledUse;
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
        _entryHolderInfo.Info.hasBeenStudied = true;
        _gameUI.HideInteractText();
    }
    #endregion

    #region MIRROR    
    public void GrabMirror(InputAction.CallbackContext context)
    {
        arm.SetActive(true);
        canDetect = false;
        _gameUI.HideInteractText();
        InputManager.Instance.InteractStarted = null;
        InputManager.Instance.DisableActions(true, false, true, false);
        LightReflector parent = _mirrorInfo.GetComponentInParent<LightReflector>();
        InputManager.Instance.MoveStarted = parent.RotateReflectorStarted;
        InputManager.Instance.MovePerformed = null;
        InputManager.Instance.MoveCanceled = parent.RotateReflectorCanceled;
        InputManager.Instance.MoveStarted += _headBobbing.OnMoveWhilePushingStarted;
        InputManager.Instance.MoveCanceled += _headBobbing.OnMoveWhilePushingCanceled;
        _headBobbing.isPushing = true;
        transform.parent = parent.transform;
        _playerCam.IsXRotClamped = true;
        if (Vector3.Angle(_mirrorInfo.forward, transform.forward) <= 90)
        {
            Quaternion rot = Quaternion.LookRotation(_mirrorInfo.forward, transform.up);
            StartCoroutine(LookToward((new Vector3(_mirrorInfo.position.x, transform.position.y, _mirrorInfo.position.z) + _mirrorInfo.forward * -0.8f), rot));
            parent.direction = -1;
        }
        else
        {
            Quaternion rot = Quaternion.LookRotation(-_mirrorInfo.forward, transform.up);
            StartCoroutine(LookToward((new Vector3(_mirrorInfo.position.x, transform.position.y, _mirrorInfo.position.z) + -_mirrorInfo.forward * -0.8f), rot));
            parent.direction = 1;
        }
    }
    public void LetOffMirror(InputAction.CallbackContext context)
    {
        arm.SetActive(false);
        transform.parent = null;
        InputManager.Instance.EnableActions(false, false, true, false);
        InputManager.Instance.MoveStarted = _playerMov.OnMoveStarted;
        InputManager.Instance.MoveStarted += _headBobbing.OnMoveStarted;
        InputManager.Instance.MovePerformed = _playerMov.OnMovePerformed;
        InputManager.Instance.MoveCanceled = _playerMov.OnMoveCanceled;
        InputManager.Instance.MoveCanceled += _headBobbing.OnMoveCanceled;
        _headBobbing.isPushing = false;
        _mirrorInfo.GetComponentInParent<LightReflector>().Reset();
        canDetect = true;
        _playerCam.IsXRotClamped = false;
    }
    IEnumerator LookToward(Vector3 pos, Quaternion rot)
    {
        while ((transform.position != pos) || (transform.rotation != rot))
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 2f * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 80f * Time.deltaTime);
            yield return null;
        }
        InputManager.Instance.EnableActions(true, false, true, false);
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
}
