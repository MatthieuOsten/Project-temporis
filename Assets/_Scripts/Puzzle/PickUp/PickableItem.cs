using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [SerializeField] ItemInfoScriptable _info;
    public ItemInfoScriptable Info { get { return _info; } private set { _info = value; } }
    Collider _collider;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickItem()
    {
        _collider.isTrigger = true;
        gameObject.layer = 2;
        _rb.isKinematic = true;
    }

    public void DropItem()
    {
        _collider.isTrigger = false;
        gameObject.layer = 9;
        _rb.isKinematic = false;
    }

    public void UseItem()
    {
        gameObject.layer = 9;
    }
}
