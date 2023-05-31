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
    private ItemInfoScriptable _itemInfo;

    public void SetButton(ItemInfoScriptable itemInfo)
    {
        _itemInfo = itemInfo;
        itemImage.sprite = itemInfo.View;
        //itemName.text = itemInfo.Name;
        button.onClick.AddListener(PickUpItem);
    }

    void PickUpItem()
    {
        _itemInfo.itemPickedUp?.Invoke(_itemInfo);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(StoreItem);
    }
    void StoreItem()
    {
        _itemInfo.itemStored?.Invoke();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(PickUpItem);
    }
}
