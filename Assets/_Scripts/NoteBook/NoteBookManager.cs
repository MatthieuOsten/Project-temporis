using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteBookManager : MonoBehaviour
{
    [Header("PAGES LISTS")]
    [SerializeField] List<NoteBookPage> _allNoteBookPages;
    [SerializeField] List<NoteBookPage> _leftNoteBookPageList, _rightNoteBookPageList;
    [Header("ENTRIES LIST")]
    [SerializeField] EntriesListScriptable _entriesList;
    [Header("PAGE PARAMETERS")]
    [SerializeField] float pageFlipSpeed;
    [SerializeField, Range(0, 0.01f)] float _pageSpacing = 0.002f;
    [SerializeField] float _rightPageMaxAngle = -160f, _leftPageMaxAngle = -20f;
    [Header("UI")]
    [SerializeField] GameObject _entryButtonPrefab;
    [SerializeField] float _buttonLength;
    [SerializeField] Transform _buttonHolder;
    List<EntryButton> _entryButtonList;
    [SerializeField] GameObject noteBookView;
    [SerializeField] NoteBookAudio _noteBookAudio;

    public Action NoteBookOpened;
    public Action NoteBookClosed;

    private void Start()
    {
        _leftNoteBookPageList = new List<NoteBookPage>();
        _rightNoteBookPageList = new List<NoteBookPage>();
        NoteBookClosed += ResetAllPages;
        _entriesList.entryAdded += OnEntryAdded;
        _entriesList.tornedEntriesAdded += OnTornedEntriesAdded;
        InputManager.Instance.OpenNoteBookStarted += OpenNoteBook;
        InputManager.Instance.CloseNoteBookStarted += CloseNoteBook;
        InitializeNoteBook();
    }

    void InitializeNoteBook()
    {
        InitializeEntryButtons();
        for(int i = _allNoteBookPages.Count-1; i >= 0; i--)
        {
            _rightNoteBookPageList.Add(_allNoteBookPages[i]);
            int pageIndex = i;
            if (!_allNoteBookPages[i].isTorned)
            {
                Debug.Log(pageIndex);
                int entryIndex = pageIndex*2;
                _allNoteBookPages[pageIndex].FrontEntry.SetEntry(_entriesList.GetEntry(entryIndex));
                _allNoteBookPages[pageIndex].BackEntry.SetEntry(_entriesList.GetEntry(entryIndex+1));
                _entryButtonList[entryIndex].SetSprite(_entriesList.GetEntry(entryIndex).EntryIcon);
                _entryButtonList[entryIndex+1].SetSprite(_entriesList.GetEntry(entryIndex+1).EntryIcon);
            }
        }
        ResetAllPages();
    }

    void OnEntryAdded(EntryScriptable newEntry, int entryIndex)
    {
        _entryButtonList[entryIndex].SetSprite(newEntry.EntryIcon);
        _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].BackEntry.HideEntry();
        _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FrontEntry.HideEntry();

        int noteBookPageIndex = (entryIndex - (entryIndex % 2))/2;
        int result = _rightNoteBookPageList.Count + (_leftNoteBookPageList.Count - noteBookPageIndex);
        if (result <= _rightNoteBookPageList.Count)
        {
            for (int i = _rightNoteBookPageList.Count - 1; i > result - 1 - entryIndex%2; i--)
            {
                Debug.Log("Droite");
                _leftNoteBookPageList.Add(_rightNoteBookPageList[i]);
                _rightNoteBookPageList.RemoveAt(i);
                _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_leftNoteBookPageList.Count-1), _leftPageMaxAngle);
            }
        }
        else
        {
            for (int i = _leftNoteBookPageList.Count - 1; i >= noteBookPageIndex + entryIndex % 2; i--)
            {
                Debug.Log("Gauche");
                _rightNoteBookPageList.Add(_leftNoteBookPageList[i]);
                _leftNoteBookPageList.RemoveAt(i);
                _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_rightNoteBookPageList.Count - 1), _rightPageMaxAngle);
            }
        }

        if(entryIndex % 2 == 0)
        {
            _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FrontEntry.SetEntry(newEntry);
        }
        else
        {
            _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].BackEntry.SetEntry(newEntry);
        }
        OpenNoteBook();
    }

    void SwitchNoteBookPages(int entryIndex)
    {
        int noteBookPageIndex = (entryIndex - (entryIndex % 2)) / 2;
        int result = _rightNoteBookPageList.Count + (_leftNoteBookPageList.Count - noteBookPageIndex);
        int delay = 0;

        if (result <= _rightNoteBookPageList.Count)
        {
            for(int i = _rightNoteBookPageList.Count-1; i > result - 1 - entryIndex % 2; i--)
            {
                _leftNoteBookPageList.Add(_rightNoteBookPageList[i]);
                _rightNoteBookPageList.RemoveAt(i);
                int index = _rightNoteBookPageList.Count - 1;
                //rightNoteBookPageList[index].ShowFrontPage();
                index = _leftNoteBookPageList.Count - 1;
                //leftNoteBookPageList[index].ShowBackPage();
                _leftNoteBookPageList[index].FlipPage(_pageSpacing * (_leftNoteBookPageList.Count-1), _leftPageMaxAngle, 400, 0.1f * delay);
                delay++;
            }
        }
        else
        {
            for(int i = _leftNoteBookPageList.Count-1; i >= noteBookPageIndex + entryIndex % 2; i--)
            {
                _rightNoteBookPageList.Add(_leftNoteBookPageList[i]);
                _leftNoteBookPageList.RemoveAt(i);
                int index = _leftNoteBookPageList.Count - 1;
                //leftNoteBookPageList[index].ShowFrontPage();
                index = _rightNoteBookPageList.Count - 1;
                //rightNoteBookPageList[index].ShowBackPage();
                _rightNoteBookPageList[index].FlipPage(_pageSpacing * (_rightNoteBookPageList.Count-1), _rightPageMaxAngle, 400,0.1f * delay);
                delay++;
            }
        }
    }

    void OnTornedEntriesAdded(EntryScriptable frontEntry, EntryScriptable backEntry)
    {
        int entryIndex = frontEntry.EntryIndex;
        _entryButtonList[entryIndex].SetSprite(frontEntry.EntryIcon);
        _entryButtonList[backEntry.EntryIndex].SetSprite(backEntry.EntryIcon);

        _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].BackEntry.HideEntry();
        _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FrontEntry.HideEntry();

        int noteBookPageIndex = (entryIndex - (entryIndex % 2)) / 2;
        int result = _rightNoteBookPageList.Count + (_leftNoteBookPageList.Count - noteBookPageIndex);
        if (result <= _rightNoteBookPageList.Count)
        {
            for (int i = _rightNoteBookPageList.Count - 1; i > result - 1; i--)
            {
                Debug.Log("Droite");
                _leftNoteBookPageList.Add(_rightNoteBookPageList[i]);
                _rightNoteBookPageList.RemoveAt(i);
                _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_leftNoteBookPageList.Count - 1), _leftPageMaxAngle);
            }
        }
        else
        {
            for (int i = _leftNoteBookPageList.Count - 1; i >= noteBookPageIndex; i--)
            {
                Debug.Log("Gauche");
                _rightNoteBookPageList.Add(_leftNoteBookPageList[i]);
                _leftNoteBookPageList.RemoveAt(i);
                _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_rightNoteBookPageList.Count - 1), _rightPageMaxAngle);
            }
        }

        _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].RepairPage(frontEntry, backEntry);

        OpenNoteBook();

        _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].BackEntry.ShowEntry();
        _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FrontEntry.ShowEntry();
    }

    public void ResetAllPages()
    {
        for(int i = 0; i< _rightNoteBookPageList.Count; i++)
        {
            _rightNoteBookPageList[i].FlipPage(_pageSpacing * i, _rightPageMaxAngle);
            _rightNoteBookPageList[i].FrontEntry.HideEntry();
            _rightNoteBookPageList[i].BackEntry.HideEntry();
        }
        for (int i = 0; i < _leftNoteBookPageList.Count; i++)
        {
            _leftNoteBookPageList[i].FlipPage(_pageSpacing * i, _leftPageMaxAngle);
            _leftNoteBookPageList[i].FrontEntry.HideEntry();
            _leftNoteBookPageList[i].BackEntry.HideEntry();
        }
        if(_rightNoteBookPageList.Count != 0)
        {
            _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FrontEntry.ShowEntry();
        }
        if(_leftNoteBookPageList.Count != 0)
        {
            _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].BackEntry.ShowEntry();
        }
    }

    public NoteBookPage GetPreviousPage(NoteBookPage currentPage)
    {
        if(_allNoteBookPages.Contains(currentPage))
        {
            int pageIndex = _allNoteBookPages.IndexOf(currentPage);
            if (pageIndex !=0)
            {
                return _allNoteBookPages[pageIndex - 1];
            }
        }
        return null;
    }

    public NoteBookPage GetNextPage(NoteBookPage currentPage)
    {
        if (_allNoteBookPages.Contains(currentPage))
        {
            int pageIndex = _allNoteBookPages.IndexOf(currentPage);
            if (pageIndex != _allNoteBookPages.Count-1)
            {
                return _allNoteBookPages[pageIndex + 1];
            }
        }
        return null;
    }

    #region INPUT FUNCTIONS
    void OpenNoteBook(InputAction.CallbackContext context)
    {
        GameUI.Instance.ShowHandCursor();
        noteBookView.SetActive(true);
        GameUI.Instance.HidePlayerScreen();
        GameUI.Instance.ShowNoteBookScreen();
        InputManager.Instance.SwitchCurrentActionMap();
        CameraManager.Instance.SetNoteBookView();
        InputManager.Instance.PointPerformed += CheckLookUp;
        _noteBookAudio.OpenNoteBookSound();
        NoteBookOpened?.Invoke();
    }
    void OpenNoteBook()
    {
        GameUI.Instance.ShowHandCursor();
        noteBookView.SetActive(true);
        GameUI.Instance.HidePlayerScreen();
        GameUI.Instance.ShowNoteBookScreen();
        CameraManager.Instance.SetNoteBookView();
        InputManager.Instance.SwitchCurrentActionMap();
        InputManager.Instance.PointPerformed += CheckLookUp;
        NoteBookOpened?.Invoke();
    }
    void CloseNoteBook(InputAction.CallbackContext context)
    {
        noteBookView.SetActive(false);
        GameUI.Instance.ShowPlayerScreen();
        GameUI.Instance.HideNoteBookScreen();
        GameUI.Instance.HideCursor();
        InputManager.Instance.SwitchCurrentActionMap();
        InputManager.Instance.PointPerformed -= CheckLookUp;
        NoteBookClosed?.Invoke();
    }
    void CheckLookUp(InputAction.CallbackContext context)
    {
        if(context.ReadValue<Vector2>().y >= 1000)
        {
            CameraManager.Instance.SetNoteBookMidView();
            GameUI.Instance.HideNoteBookScreen();
            InputManager.Instance.PointPerformed -= CheckLookUp;
            InputManager.Instance.PointPerformed += CheckLookDown;
        }
    }
    void CheckLookDown(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().y <= 100)
        {
            CameraManager.Instance.SetNoteBookView();
            GameUI.Instance.ShowNoteBookScreen();
            InputManager.Instance.PointPerformed -= CheckLookDown;
            InputManager.Instance.PointPerformed += CheckLookUp;
        }
    }
    #endregion

    #region BUTTON FUNCTIONS
    public void TurnRight()
    {
        if(_leftNoteBookPageList.Count != 0)
        {
            _rightNoteBookPageList.Add(_leftNoteBookPageList[_leftNoteBookPageList.Count - 1]);
            _leftNoteBookPageList.RemoveAt(_leftNoteBookPageList.Count - 1);
            _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_rightNoteBookPageList.Count - 1), _rightPageMaxAngle, pageFlipSpeed);
        }
    }

    public void TurnLeft()
    {
        if (_rightNoteBookPageList.Count != 0)
        {
            _leftNoteBookPageList.Add(_rightNoteBookPageList[_rightNoteBookPageList.Count - 1]);
            _rightNoteBookPageList.RemoveAt(_rightNoteBookPageList.Count - 1);
            _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(_pageSpacing * (_leftNoteBookPageList.Count - 1), _leftPageMaxAngle, pageFlipSpeed);
        }
    }

    private void InitializeEntryButtons()
    {
        _entryButtonList = new List<EntryButton>();
        int nbButtons = _allNoteBookPages.Count * 2;
        float buttonSpacing = _buttonLength / nbButtons;
        float buttonStartPos = -buttonSpacing * ((nbButtons - 1) / 2);
        if (nbButtons % 2 == 0)
        {
            buttonStartPos -= buttonSpacing / 2;
        }
        for (int i = 0; i < nbButtons; i++)
        {
            int index = i;
            EntryButton newEntryButton = Instantiate(_entryButtonPrefab, _buttonHolder).GetComponent<EntryButton>();
            newEntryButton.Initialize(buttonStartPos + buttonSpacing * i, i + 1);
            newEntryButton.entryButton.onClick.AddListener(() => { SwitchNoteBookPages(index); });
            _entryButtonList.Add(newEntryButton);
        }
    }
    public void LookUp()
    {
        CameraManager.Instance.SetNoteBookMidView();
        GameUI.Instance.HideNoteBookScreen();
        InputManager.Instance.PointPerformed -= CheckLookUp;
        InputManager.Instance.PointPerformed += CheckLookDown;
    }
    #endregion
}
