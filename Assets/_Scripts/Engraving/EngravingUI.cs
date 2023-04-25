using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EngravingUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _engravingTranslate;
    [SerializeField] Image _engravingIcon;
    private EngravingScriptable _engraving;
    private PageList _pageList;

    /// <summary>
    /// Set engraving Scriptable info in the NoteBook gameObject
    /// </summary>
    /// <param name="engraving"></param>
    public void Set(EngravingScriptable engraving)
    {
        _engraving = engraving;
        // Recupere les infos du Scriptable et les place dans l'UI 
        _engravingIcon.sprite = _engraving.engravingSprite;
        _engravingTranslate.text = _engraving.engravingTranslate;
    }

    public void Set(PageList pageList, int currentIndex)
    {
        _pageList = pageList;
        int index = _pageList._allPage.IndexOf(_pageList._allPage[currentIndex]);
        Debug.Log(index);
        _engravingIcon.sprite = _pageList._allPage[index]._image;
        _engravingTranslate.text = _pageList._allPage[index]._text;
    }
}
