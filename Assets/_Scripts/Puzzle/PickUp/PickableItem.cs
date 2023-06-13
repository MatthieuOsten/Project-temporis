using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickableItem : MonoBehaviour
{
    [SerializeField] ItemInfoScriptable _info;
    public ItemInfoScriptable Info { get { return _info; } private set { _info = value; } }
    Collider _collider;
    [SerializeField] GameObject _pickedView, _droppedView;

    public Action itemPicked, itemStored;
    public Action<PickableItem> itemPickedUp, itemPickedFromReceiver;
    public Action<Transform> itemUsed;

    public bool picked;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        itemPicked += OnItemPicked;
        itemPickedFromReceiver += OnItemPickedFromReceiver;
        itemPickedUp += OnItemPickedUp;
        itemUsed += OnItemUsed;
        itemStored += OnItemStored;
    }

    public void OnItemPicked()
    {
        _collider.isTrigger = true;
        gameObject.layer = 2;
        _droppedView.SetActive(false);
        picked = true;
    }

    public void OnItemPickedFromReceiver(PickableItem item)
    {
        gameObject.layer = 2;
        _pickedView.SetActive(false);
    }

    public void OnItemPickedUp(PickableItem item)
    {
        _pickedView.SetActive(true);
        InputManager.Instance.OpenInventoryStarted?.Invoke(new InputAction.CallbackContext());
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
