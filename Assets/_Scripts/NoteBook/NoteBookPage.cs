using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookPage : MonoBehaviour
{
    [SerializeField] NoteBookEntry _frontEntry, _backEntry;
    public NoteBookEntry FrontEntry { get { return _frontEntry; } }
    public NoteBookEntry BackEntry { get { return _backEntry; } }
    [SerializeField] Transform pagePivot;
    public bool isTorned = false;
    [SerializeField] GameObject _repairedView, _tornedView;
    public NoteBookManager noteBookManager;

    public void FlipPage(float posZ, float rotY)
    {
        pagePivot.localPosition = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        pagePivot.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
    }

    public void FlipPage(float posZ, float rotY, float speed)
    {
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, 0, rotY != -20f));
    }

    public void FlipPage(float posZ, float rotY, float speed, float delay)
    {
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, delay, rotY != -20f));
    }

    public void RepairPage(EntryScriptable frontEntryInfo, EntryScriptable backEntryInfo)
    {
        _frontEntry.SetEntry(frontEntryInfo);
        _backEntry.SetEntry(backEntryInfo);
        _tornedView.SetActive(false);
        _repairedView.SetActive(true);
        isTorned = false;
    }

    IEnumerator FlipPivot(Vector3 pos, Quaternion rot, float speed, float delay, bool turnRight)
    {
        yield return new WaitForSeconds(delay);
        GameUI.Instance.isLocked = true;
        NoteBookPage previousPage = noteBookManager.GetPreviousPage(this);
        NoteBookPage nextPage = noteBookManager.GetNextPage(this);
        Debug.Log(turnRight);
        if (turnRight)
        {
            _frontEntry.ShowEntry();
            if(previousPage != null)
            {
                previousPage.BackEntry.ShowEntry();
            }
        }
        else
        {
            _backEntry.ShowEntry();
            if(nextPage != null)
            {
                nextPage.FrontEntry.ShowEntry();
            }
        }
        while (pagePivot.localPosition != pos || pagePivot.localRotation != rot)
        {
            pagePivot.localPosition = Vector3.MoveTowards(pagePivot.localPosition, pos, Time.deltaTime *(speed/1000));
            pagePivot.localRotation = Quaternion.RotateTowards(pagePivot.localRotation, rot, Time.deltaTime * speed);
            yield return null;
        }
        pagePivot.localPosition = pos;
        pagePivot.localRotation = rot;
        if(turnRight)
        {
            _backEntry.HideEntry();
            if(nextPage != null)
            {
                nextPage.FrontEntry.HideEntry();
            }
        }
        else
        {
            _frontEntry.HideEntry();
            if(previousPage != null)
            {
                previousPage.BackEntry.HideEntry();
            }
        }
        GameUI.Instance.isLocked = false;
    }
}
