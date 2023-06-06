using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryButton : MonoBehaviour
{
    public Button entryButton;
    public TextMeshProUGUI entryIndexText;
    public Image entrySprite;

    public void Initialize(float xPos, int buttonIndex)
    {
        transform.localPosition = new Vector3(xPos, 0, 0);
        entryIndexText.text = buttonIndex.ToString();
    }

    public void SetSprite(Sprite entryIllustration)
    {
        entrySprite.sprite = entryIllustration;
        entrySprite.color = Color.white;
    }
}
