using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickableItem : MonoBehaviour
{
    [SerializeField] ItemInfoScriptable _info;
    public ItemInfoScriptable Info { get { return _info; } private set { _info = value; } }
    Collider _collider;
    [SerializeField] GameObject _pickedView, _droppedView;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _info.itemPicked += OnItemPicked;
        _info.itemPickedFromReceiver += OnItemPickedFromReceiver;
        _info.itemPickedUp += OnItemPickedUp;
        _info.itemUsed += OnItemUsed;
        _info.itemStored += OnItemStored;
    }

    public void OnItemPicked()
    {
        _collider.isTrigger = true;
        gameObject.layer = 2;
        _droppedView.SetActive(false);
    }

    public void OnItemPickedFromReceiver(ItemInfoScriptable itemInfo)
    {
        gameObject.layer = 2;
        _pickedView.SetActive(false);
    }

    public void OnItemPickedUp(ItemInfoScriptable itemInfo)
    {
        _pickedView.SetActive(true);
        InputManager.Instance.InventoryStarted?.Invoke(new InputAction.CallbackContext());
        _info.used = false;
    }

    public void OnItemStored()
    {
        _pickedView.SetActive(false);
    }

    public void OnItemUsed(Transform target)
    {
        gameObject.layer = 11;
        transform.parent = target;
        StartCoroutine(MoveTo(target.position, transform));
        _info.used = true;
    }

    IEnumerator MoveTo(Vector3 pos, Transform target)
    {
        while (target.position != pos)
        {
            target.position = Vector3.MoveTowards(target.position, pos, 8f * Time.deltaTime);
            yield return null;
        }
    }
}
