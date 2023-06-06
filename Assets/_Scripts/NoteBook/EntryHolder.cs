using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHolder : MonoBehaviour
{
    [SerializeField] protected EntryInfoScriptable _info;
    public EntryInfoScriptable Info { get { return _info; } }

    // Start is called before the first frame update
    void Start()
    {
        _info.hasBeenStudied = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
