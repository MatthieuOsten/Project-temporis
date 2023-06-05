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

    void OnWrongItemReceived(PickableItem item)
    {
        nbReceveirsUsed++;
        WinCheck();
        item.itemPickedFromReceiver += OnItemPickedFromReceiver;
    }

    void OnRightItemReceived(PickableItem item)
    {
        nbReceveirsUsed++;
        nbReceiversCompleted++;
        WinCheck();
        item.itemPickedFromReceiver += OnItemPickedFromReceiver;
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

    public void OnItemPickedFromReceiver(PickableItem item)
    {
        ItemReceiver linckedReceiver = FindLinckedReceiver(item.Info);
        if(item.Info.Id == linckedReceiver.linckedItemId)
        {
            nbReceiversCompleted--;
        }
        nbReceveirsUsed--;
        linckedReceiver.linckedItemInfo = null;
        item.itemPickedFromReceiver -= OnItemPickedFromReceiver;
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
