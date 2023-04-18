using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePrint : Interactable
{
    private bool _hasBeenReaden;

    protected override void TextToTranslate()
    {
        Debug.Log(TranslateText);
    }
}
