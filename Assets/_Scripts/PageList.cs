using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteBookData", menuName = "NoteBook/NoteBookScriptable")]
public class PageList : ScriptableObject
{
    public struct Page
    {
        public string _name;
        public bool _activate;
        public Sprite _image;
        public string _text;
    }

    public List<Page> _allPage;
}
