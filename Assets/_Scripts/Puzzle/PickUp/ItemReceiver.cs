using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public int linckedItemId;
    public Transform itemPosition;
    public ItemInfoScriptable linckedItemInfo;

    public Action<PickableItem> WrongItemReceived, RightItemReceived;
}
