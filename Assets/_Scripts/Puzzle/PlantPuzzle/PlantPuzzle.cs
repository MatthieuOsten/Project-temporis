using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPuzzle : MonoBehaviour
{
    [SerializeField] ItemReceiver[] itemReceivers;
    private int nbReceveirsUsed, nbReceiversCompleted; 

    // Start is called before the first frame update
    void Start()
    {
        nbReceveirsUsed = 0;
        for(int i = 0; i<itemReceivers.Length; i++)
        {
            itemReceivers[i].WrongItemReceived += OnWrongItemReceived;
            itemReceivers[i].RightItemReceived += OnRightItemReceived;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnWrongItemReceived(ItemInfoScriptable itemInfo)
    {
        nbReceveirsUsed++;
        WinCheck();
        itemInfo.itemPickedFromReceiver += OnItemPickedFromReceiver;
    }

    void OnRightItemReceived(ItemInfoScriptable itemInfo)
    {
        nbReceveirsUsed++;
        nbReceiversCompleted++;
        WinCheck();
        itemInfo.itemPickedFromReceiver += OnItemPickedFromReceiver;
    }

    void WinCheck()
    {
        if (nbReceveirsUsed == itemReceivers.Length)
        {
            if(nbReceiversCompleted == itemReceivers.Length)
            {
                Debug.Log("You Win!");
            }
        }
    }

    public void OnItemPickedFromReceiver(ItemInfoScriptable itemInfo)
    {
        ItemReceiver linckedReceiver = FindLinckedReceiver(itemInfo);
        if(itemInfo.Id == linckedReceiver.linckedItemId)
        {
            nbReceiversCompleted--;
        }
        nbReceveirsUsed--;
        linckedReceiver.linckedItemInfo = null;
        itemInfo.itemPickedFromReceiver -= OnItemPickedFromReceiver;
    }

    ItemReceiver FindLinckedReceiver(ItemInfoScriptable itemInfo)
    {
        for(int i = 0; i < itemReceivers.Length; i++)
        {
            if(itemReceivers[i].linckedItemInfo == itemInfo)
            {
                return itemReceivers[i];
            }
        }
        return null;
    }
}
