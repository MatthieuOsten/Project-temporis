using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteBookManager : MonoBehaviour
{
    [SerializeField] List<NoteBookPage> leftNoteBookPageList, rightNoteBookPageList;
    [SerializeField] EntriesListScriptable _entriesList;
    [SerializeField] EngravingScriptable _engravingScriptableTest;
    [SerializeField] float pageFlipSpeed;
    [SerializeField] Button button;

    private void Start()
    {
        _entriesList.Clear();
        _entriesList.EntryAdded += OnPageAdded;
        button.onClick.AddListener(() => { SwitchNoteBookPages(0); });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Add");
            AddPage();
        }
    }

    void OnPageAdded(EngravingScriptable newEntry, int entryIndex)
    {
        Debug.Log(entryIndex);
        int noteBookPageIndex = (entryIndex - (entryIndex % 2))/2;
        Debug.Log(noteBookPageIndex);

        int result = rightNoteBookPageList.Count + (leftNoteBookPageList.Count - noteBookPageIndex);
        Debug.Log(result);
        if (result <= rightNoteBookPageList.Count)
        {
            for (int i = rightNoteBookPageList.Count - 1; i > result - 1 - entryIndex%2; i--)
            {
                Debug.Log("Droite");
                leftNoteBookPageList.Add(rightNoteBookPageList[i]);
                rightNoteBookPageList.RemoveAt(i);
                leftNoteBookPageList[leftNoteBookPageList.Count - 1].FlipPage(0.002f * (leftNoteBookPageList.Count-1), -20);
            }
        }
        else
        {
            for (int i = leftNoteBookPageList.Count - 1; i >= noteBookPageIndex + entryIndex % 2; i--)
            {
                Debug.Log("Gauche");
                rightNoteBookPageList.Add(leftNoteBookPageList[i]);
                leftNoteBookPageList.RemoveAt(i);
                rightNoteBookPageList[rightNoteBookPageList.Count - 1].FlipPage(0.002f * (rightNoteBookPageList.Count - 1), -160);
            }
        }

        if(entryIndex % 2 == 0)
        {
            rightNoteBookPageList[rightNoteBookPageList.Count - 1].SetFrontPage(newEntry);
        }
        else
        {
            leftNoteBookPageList[leftNoteBookPageList.Count-1].SetBackPage(newEntry);
        }
    }

    void SwitchNoteBookPages(int entryIndex)
    {
        // A modifier pour gérer l'affichage et le désaffichage des pages

        int noteBookPageIndex = (entryIndex - (entryIndex % 2)) / 2;
        int result = rightNoteBookPageList.Count + (leftNoteBookPageList.Count - noteBookPageIndex);
        int delay = 0;

        if (result <= rightNoteBookPageList.Count)
        {
            for(int i = rightNoteBookPageList.Count-1; i > result - 1 - entryIndex % 2; i--)
            {

                leftNoteBookPageList.Add(rightNoteBookPageList[i]);
                rightNoteBookPageList.RemoveAt(i);
                int index = rightNoteBookPageList.Count - 1;
                //rightNoteBookPageList[index].ShowFrontPage();
                index = leftNoteBookPageList.Count - 1;
                //leftNoteBookPageList[index].ShowBackPage();
                leftNoteBookPageList[index].FlipPage(0.002f * (leftNoteBookPageList.Count-1), -20f, 400, 0.1f * delay);
                delay++;
            }
        }
        else
        {
            for(int i = leftNoteBookPageList.Count-1; i >= noteBookPageIndex + entryIndex % 2; i--)
            {
                rightNoteBookPageList.Add(leftNoteBookPageList[i]);
                leftNoteBookPageList.RemoveAt(i);
                int index = leftNoteBookPageList.Count - 1;
                //leftNoteBookPageList[index].ShowFrontPage();
                index = rightNoteBookPageList.Count - 1;
                //rightNoteBookPageList[index].ShowBackPage();
                rightNoteBookPageList[index].FlipPage(0.002f * (rightNoteBookPageList.Count-1), -160f, 400,0.1f * delay);
                delay++;
            }
        }
    }

    void AddPage()
    {
        _entriesList.AddPage(_engravingScriptableTest);
    }

    #region ButtonFunctions
    public void TurnRight()
    {
        if(leftNoteBookPageList.Count != 0)
        {
            rightNoteBookPageList.Add(leftNoteBookPageList[leftNoteBookPageList.Count - 1]);
            leftNoteBookPageList.RemoveAt(leftNoteBookPageList.Count - 1);
            rightNoteBookPageList[rightNoteBookPageList.Count - 1].FlipPage(0.005f * (rightNoteBookPageList.Count - 1), -160, pageFlipSpeed);
        }
    }

    public void TurnLeft()
    {
        if (rightNoteBookPageList.Count != 0)
        {
            leftNoteBookPageList.Add(rightNoteBookPageList[rightNoteBookPageList.Count - 1]);
            rightNoteBookPageList.RemoveAt(rightNoteBookPageList.Count - 1);
            leftNoteBookPageList[leftNoteBookPageList.Count - 1].FlipPage(0.005f * (leftNoteBookPageList.Count - 1), -20, pageFlipSpeed);
        }
    }
    #endregion
}
