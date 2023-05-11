using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] GameUI _gameUI;
    [SerializeField] EngravingUI _noteBook;
    [SerializeField] PageList _pageInventory;
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _interactLayers;
    [SerializeField] private LayerMask _itemReceiverLayer;
    [SerializeField] private Transform _pickUpPoint;

    private Engraving _engravingInfo;
    private Transform _mirrorInfo;
    private Transform _pickUpInfo;
    private ItemInfoScriptable _currentItemInfo;
    private ItemReceiver _itemReceiverInfo;

    bool canDetect = true;
    bool isHoldingItem = false;

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
                if (/*hitInfo.collider.GetComponent<Engraving>() != null*/ hitInfo.collider.TryGetComponent<Engraving>(out _engravingInfo))
                {
                    /*_engravingInfo = hitInfo.collider.GetComponent<Engraving>(); // var to store engraving info*/
                    if (!_engravingInfo.EngravingScriptable.HasBeenStudied)
                    {
                        _gameUI.ShowInteractText("Press E to interact");
                        InputManager.Instance.InteractStarted = TranslatePrint;
                    }
                }
            }
            else if (hitInfo.transform.gameObject.layer == 8)
            {
                _mirrorInfo = hitInfo.transform;
                _gameUI.ShowInteractText("Press E to grab");
                InputManager.Instance.InteractStarted = GrabMirror;
                InputManager.Instance.InteractCancelled = LetOffMirror;
            }
            else if (hitInfo.transform.gameObject.layer == 9)
            {
                _gameUI.ShowInteractText("Press E to pick");
                _pickUpInfo = hitInfo.transform;
                InputManager.Instance.InteractStarted = PickUpItem;
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
            _gameUI.ShowInteractText("Press E to use");
            _itemReceiverInfo = hitInfo.transform.GetComponent<ItemReceiver>();
            InputManager.Instance.InteractStarted = UseItem;
        }
        else
        {
            _gameUI.HideInteractText();
            InputManager.Instance.InteractStarted = DropItem;
        }
    }
    #endregion

    #region PRINT
    public void TranslatePrint(InputAction.CallbackContext context)
    {
        EngravingScriptable engravingScriptableToAdd = _engravingInfo.EngravingScriptable;
        _noteBook.Set(engravingScriptableToAdd);
        _engravingInfo.EngravingScriptable.HasBeenStudied = true;
        _pageInventory.SetPageInfo(engravingScriptableToAdd); //ajoute la page à la liste
        InputManager.Instance.InteractStarted -= TranslatePrint;
    }
    #endregion

    #region MIRROR    
    public void GrabMirror(InputAction.CallbackContext context)
    {
        canDetect = false;
        _gameUI.HideInteractText();
        InputManager.Instance.InteractStarted = null;
        InputManager.Instance.DisableActions(true, false, true, false);
        LightReflector parent = _mirrorInfo.GetComponentInParent<LightReflector>();
        InputManager.Instance.MoveChanged = parent.RotateReflector;
        transform.parent = parent.transform;
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
        transform.parent = null;
        InputManager.Instance.EnableActions(false, false, true, false);
        InputManager.Instance.MoveChanged = GetComponent<PlayerMovement>().OnMove;
        _mirrorInfo.GetComponentInParent<LightReflector>().Reset();
        canDetect = true;
        transform.GetComponent<PlayerCamera>().IsXRotClamped = false;
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
        transform.GetComponent<PlayerCamera>().IsXRotClamped = true;
    }
    #endregion

    #region PICKUP
    void PickUpItem(InputAction.CallbackContext context)
    {
        isHoldingItem = true;
        Reset();
        StartCoroutine(MoveTo(_pickUpPoint.position, _pickUpInfo));
        PickableItem itemScript = _pickUpInfo.GetComponent<PickableItem>();
        itemScript.PickItem();
        _currentItemInfo = itemScript.Info;
        _gameUI.ShowItem(_currentItemInfo.View);
        _pickUpInfo.parent = _pickUpPoint;
        InputManager.Instance.InteractStarted = DropItem;
    }
    void DropItem(InputAction.CallbackContext context)
    {
        isHoldingItem = false;
        PickableItem itemScript = _pickUpInfo.GetComponent<PickableItem>();
        itemScript.DropItem();
        _pickUpInfo.parent = null;
        _gameUI.HideItem();
    }
    void UseItem(InputAction.CallbackContext context)
    {
        StartCoroutine(MoveTo(_itemReceiverInfo.itemPosition.position, _pickUpInfo));
        PickableItem itemScript = _pickUpInfo.GetComponent<PickableItem>();
        itemScript.UseItem();
        _pickUpInfo.parent = null;
        isHoldingItem = false;
        _gameUI.HideItem();
        Reset();
    }
    IEnumerator MoveTo(Vector3 pos, Transform target)
    {
        while (target.position != pos)
        {
            target.position = Vector3.MoveTowards(target.position, pos, 8f * Time.deltaTime);
            yield return null;
        }
    }
    #endregion
}
