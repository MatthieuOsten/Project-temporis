using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornPageHolder : MonoBehaviour
{
    [SerializeField] EntryScriptable _frontEntryInfo, _backEntryInfo;
    public EntryScriptable FrontEntryInfo { get { return _frontEntryInfo; } }
    public EntryScriptable BackEntryInfo { get { return _backEntryInfo; } }
}
