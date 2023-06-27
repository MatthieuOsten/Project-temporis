using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryHolder : MonoBehaviour
{
    [SerializeField] protected EntryScriptable _info;
    public EntryScriptable Info { get { return _info; } }
}
