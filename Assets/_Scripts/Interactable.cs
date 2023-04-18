using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private string _translateText;

    public string TranslateText
    {
        get { return _translateText; }

        set { _translateText = value; }
    }

    public void BaseInteract()
    {
        TextToTranslate();
    }

    protected virtual void TextToTranslate() { }
}
