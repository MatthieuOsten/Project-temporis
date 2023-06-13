using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornPageHolder : MonoBehaviour
{
    [SerializeField] EntryInfoScriptable _frontEntryInfo, _backEntryInfo;
    public EntryInfoScriptable FrontEntryInfo { get { return _frontEntryInfo; } }
    public EntryInfoScriptable BackEntryInfo { get { return _backEntryInfo; } }
}
