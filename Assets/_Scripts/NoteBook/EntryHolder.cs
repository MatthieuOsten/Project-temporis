using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHolder : MonoBehaviour
{
    [SerializeField] protected EntryInfoScriptable _info;
    public EntryInfoScriptable Info { get { return _info; } }
}
