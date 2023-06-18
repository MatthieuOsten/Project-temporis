using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NoteBookManager : MonoBehaviour
{
    [SerializeField] List<NoteBookPage> _leftNoteBookPageList, _rightNoteBookPageList;
    [SerializeField] EntriesListScriptable _entriesList;
    [SerializeField] float pageFlipSpeed;

    [SerializeField] float _buttonLength;
    [SerializeField] GameObject _entryButtonPrefab;
    [SerializeField] Transform _buttonHolder;
    List<EntryButton> _entryButtonList;
    [SerializeField] Transform noteBookPOV;
    [SerializeField] GameObject noteBookView;

    public Action NoteBookOpened;
    public Action NoteBookClosed;

    private void Start()
    {
        _entriesList.entryAdded += OnEntryAdded;
        _entriesList.tornedEntriesAdded += OnTornedEntriesAdded;
        InputManager.Instance.OpenNoteBookStarted += OpenNoteBook;
        InputManager.Instance.CloseNoteBookStarted += CloseNoteBook;
        InitializeNoteBook();
    }

    void InitializeNoteBook()
    {
        InitializeEntryButtons();
        int nbEntries = _rightNoteBookPageList.Count*2;
        for(int i =0; i < nbEntries; i++)
        {
            EntryScriptable currentEntry = _entriesList.GetEntry(i);
            if (currentEntry)
            {
                int pageIndex = _rightNoteBookPageList.Count - 1 - ((i - i % 2) / 2);
                if(i%2 == 0)
                {
                    _rightNoteBookPageList[pageIndex].FrontEntry.SetEntry(currentEntry);

                }
                else
                {
                    _rightNoteBookPageList[pageIndex].BackEntry.SetEntry(currentEntry);
                }
                _entryButtonList[i].SetSprite(currentEntry.EntryIcon);
            }
        }
    }

    void OnEntryAdded(EntryScriptable newEntry, int entryIndex)
    {
        _entryButtonList[entryIndex].SetSprite(newEntry.EntryIcon);
        int noteBookPageIndex = (entryIndex - (entryIndex % 2))/2;

        int result = _rightNoteBookPageList.Count + (_leftNoteBookPageList.Count - noteBookPageIndex);
        if (result <= _rightNoteBookPageList.Count)
        {
            for (int i = _rightNoteBookPageList.Count - 1; i > result - 1 - entryIndex%2; i--)
            {
                Debug.Log("Droite");
                _leftNoteBookPageList.Add(_rightNoteBookPageList[i]);
                _rightNoteBookPageList.RemoveAt(i);
                _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(0.002f * (_leftNoteBookPageList.Count-1), -20);
            }
        }
        else
        {
            for (int i = _leftNoteBookPageList.Count - 1; i >= noteBookPageIndex + entryIndex % 2; i--)
            {
                Debug.Log("Gauche");
                _rightNoteBookPageList.Add(_leftNoteBookPageList[i]);
                _leftNoteBookPageList.RemoveAt(i);
                _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(0.002f * (_rightNoteBookPageList.Count - 1), -160);
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
        OpenNoteBook(new InputAction.CallbackContext());
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
                _leftNoteBookPageList[index].FlipPage(0.002f * (_leftNoteBookPageList.Count-1), -20f, 400, 0.1f * delay);
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
                _rightNoteBookPageList[index].FlipPage(0.002f * (_rightNoteBookPageList.Count-1), -160f, 400,0.1f * delay);
                delay++;
            }
        }
    }

    void OnTornedEntriesAdded(EntryScriptable frontEntry, EntryScriptable backEntry)
    {
        int entryIndex = frontEntry.EntryIndex;
        _entryButtonList[entryIndex].SetSprite(frontEntry.EntryIcon);
        _entryButtonList[backEntry.EntryIndex].SetSprite(backEntry.EntryIcon);
        int noteBookPageIndex = (entryIndex - (entryIndex % 2)) / 2;

        int result = _rightNoteBookPageList.Count + (_leftNoteBookPageList.Count - noteBookPageIndex);
        if (result <= _rightNoteBookPageList.Count)
        {
            for (int i = _rightNoteBookPageList.Count - 1; i > result - 1; i--)
            {
                Debug.Log("Droite");
                _leftNoteBookPageList.Add(_rightNoteBookPageList[i]);
                _rightNoteBookPageList.RemoveAt(i);
                _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(0.002f * (_leftNoteBookPageList.Count - 1), -20);
            }
        }
        else
        {
            for (int i = _leftNoteBookPageList.Count - 1; i >= noteBookPageIndex; i--)
            {
                Debug.Log("Gauche");
                _rightNoteBookPageList.Add(_leftNoteBookPageList[i]);
                _leftNoteBookPageList.RemoveAt(i);
                _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(0.002f * (_rightNoteBookPageList.Count - 1), -160);
            }
        }

        _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].RepairPage(frontEntry, backEntry);

        OpenNoteBook();
    }

    #region INPUT FUNCTIONS
    void OpenNoteBook(InputAction.CallbackContext context)
    {
        GameUI.Instance.ShowHandCursor();
        noteBookView.SetActive(true);
        GameUI.Instance.HidePlayerScreen();
        GameUI.Instance.ShowNoteBookScreen();
        StartCoroutine(LookToward(CameraUtility.Camera.transform, noteBookPOV.localPosition, noteBookPOV.localRotation));
        InputManager.Instance.SwitchCurrentActionMap();
        NoteBookOpened?.Invoke();
    }
    void OpenNoteBook()
    {
        GameUI.Instance.ShowHandCursor();
        noteBookView.SetActive(true);
        GameUI.Instance.HidePlayerScreen();
        GameUI.Instance.ShowNoteBookScreen();
        StartCoroutine(LookToward(CameraUtility.Camera.transform, noteBookPOV.localPosition, noteBookPOV.localRotation));
        InputManager.Instance.SwitchCurrentActionMap();
        NoteBookOpened?.Invoke();
    }
    void CloseNoteBook(InputAction.CallbackContext context)
    {
        noteBookView.SetActive(false);
        GameUI.Instance.ShowPlayerScreen();
        GameUI.Instance.HideNoteBookScreen();
        InputManager.Instance.SwitchCurrentActionMap();
        NoteBookClosed?.Invoke();
        GameUI.Instance.HideCursor();
    }
    #endregion

    #region BUTTON FUNCTIONS
    public void TurnRight()
    {
        if(_leftNoteBookPageList.Count != 0)
        {
            _rightNoteBookPageList.Add(_leftNoteBookPageList[_leftNoteBookPageList.Count - 1]);
            _leftNoteBookPageList.RemoveAt(_leftNoteBookPageList.Count - 1);
            _rightNoteBookPageList[_rightNoteBookPageList.Count - 1].FlipPage(0.005f * (_rightNoteBookPageList.Count - 1), -160, pageFlipSpeed);
        }
    }

    public void TurnLeft()
    {
        if (_rightNoteBookPageList.Count != 0)
        {
            _leftNoteBookPageList.Add(_rightNoteBookPageList[_rightNoteBookPageList.Count - 1]);
            _rightNoteBookPageList.RemoveAt(_rightNoteBookPageList.Count - 1);
            _leftNoteBookPageList[_leftNoteBookPageList.Count - 1].FlipPage(0.005f * (_leftNoteBookPageList.Count - 1), -20, pageFlipSpeed);
        }
    }

    private void InitializeEntryButtons()
    {
        _entryButtonList = new List<EntryButton>();
        int nbButtons = _rightNoteBookPageList.Count * 2;
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
    #endregion

    IEnumerator LookToward(Transform target, Vector3 pos, Quaternion rot)
    {
        while ((target.localPosition != pos) || (target.localRotation != rot))
        {
            target.localPosition = Vector3.MoveTowards(target.localPosition, pos, Time.deltaTime);
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, 60f * Time.deltaTime);
            yield return null;
        }
    }
}
