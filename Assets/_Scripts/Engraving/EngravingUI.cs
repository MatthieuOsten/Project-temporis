using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EngravingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _engravingTranslate;
    [SerializeField] Image _engravingIcon;
    private EngravingScriptable _engraving;

    public void Set(EngravingScriptable engraving)
    {
        _engraving = engraving;
        // Recupere les infos du Scriptable et les place dans l'UI 
        _engravingIcon.sprite = _engraving.engravingSprite;
        _engravingTranslate.text = _engraving.engravingTranslate;
    }
}
