using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBookEditableElement : MonoBehaviour
{
    [SerializeField] protected EditIllustrationButton _linckedButton;
    [SerializeField] protected NoteBookManager _notebookManager;
    protected int _initialIndex, _currentIndex;
    protected bool _added;

    protected virtual void OnIllustrationEdited(int index)
    {
        _currentIndex = index;
        if (_initialIndex != _currentIndex)
        {
            _notebookManager.NoteBookClosed += OnNoteBookClosed;
        }
        else
        {
            _notebookManager.NoteBookClosed -= OnNoteBookClosed;
        }
    }

    protected virtual void OnNoteBookOpened()
    {
        _initialIndex = _currentIndex;
        _added = false;
    }

    protected abstract void OnNoteBookClosed();

    protected void Start()
    {
        _initialIndex = _linckedButton.GetIllustrationIndex();
        _currentIndex = _initialIndex;
        _linckedButton.illustrationEdited += OnIllustrationEdited;
        _notebookManager.NoteBookOpened += OnNoteBookOpened;
    }

}
