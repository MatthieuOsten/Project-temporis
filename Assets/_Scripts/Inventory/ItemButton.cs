using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Button button;
    [SerializeField] Image itemImage;
    public TextMeshProUGUI itemName;
    [HideInInspector] public PickableItem linckedItem;

    public void SetButton(PickableItem item)
    {
        linckedItem = item;
        ItemInfoScriptable itemInfo = item.Info;
        //itemName.text = itemInfo.Name;
        itemImage.sprite = itemInfo.View;
        item.itemPickedUp += OnItemPickedUp;
        item.itemStored += OnItemStored;
        button.onClick.AddListener(PickUpItem);
    }

    void PickUpItem()
    {
        linckedItem.itemPickedUp?.Invoke(linckedItem);
    }
    void StoreItem()
    {
        linckedItem.itemStored?.Invoke();
    }

    public void OnItemStored()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PickUpItem);
    }

    public void OnItemPickedUp(PickableItem item)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StoreItem);
    }
}
