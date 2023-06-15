using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteBookData", menuName = "NoteBook/NoteBookScriptable")]
public class PageList : ScriptableObject
{
    [System.Serializable]
    public struct Page
    {
        public string _name;
        public bool _activate;
        public Sprite _image;
        public string _text;
    }

    public List<Page> _allPage;

    private void OnEnable()
    {
        if (_allPage != null) //Si la liste de page n'est pas a null
        {
            _allPage.Clear(); //On la Clear
        }
        else
        {
            _allPage = new List<Page>(); //Si la liste est nulle, on en creer une nouvelle
        }
    }

    public void AddPage(Page page)
    {
        _allPage.Add(page);
    }

    public void SetPageInfo(EngravingScriptable engraving)
    {
        Page newPage = new Page();
        newPage._activate = engraving.HasBeenStudied;
        newPage._text = engraving.engravingTranslate;
        newPage._image = engraving.engravingSprite;
        AddPage(newPage);
    }

    public void SetPageInfo(EventTriggerScriptableEnviroNoteBook trigger)
    {
        Page newPage = new Page();
        newPage._activate = trigger._activate;
        newPage._text = trigger._description;
        newPage._image = trigger._draw;
        AddPage(newPage);
        Debug.Log(_allPage.Count + "page ajouté");
    }
}
