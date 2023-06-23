using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteBookPage : MonoBehaviour
{
    [SerializeField] NoteBookEntry _frontEntry, _backEntry;
    public NoteBookEntry FrontEntry { get { return _frontEntry; } }
    public NoteBookEntry BackEntry { get { return _backEntry; } }
    [SerializeField] Transform pagePivot;
    bool _isTorned = false;
    [SerializeField] GameObject _repairedView, _tornedView;

    public void FlipPage(float posZ, float rotY)
    {
        pagePivot.localPosition = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        pagePivot.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
    }

    public void FlipPage(float posZ, float rotY, float speed)
    {
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, 0));
    }

    public void FlipPage(float posZ, float rotY, float speed, float delay)
    {
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, delay));
    }

    public void RepairPage(EntryScriptable frontEntryInfo, EntryScriptable backEntryInfo)
    {
        _frontEntry.SetEntry(frontEntryInfo);
        _backEntry.SetEntry(backEntryInfo);
        _tornedView.SetActive(false);
        _repairedView.SetActive(true);
    }

    IEnumerator FlipPivot(Vector3 pos, Quaternion rot, float speed, float delay)
    {
        yield return new WaitForSeconds(delay);
        while (pagePivot.localPosition != pos || pagePivot.localRotation != rot)
        {
            pagePivot.localPosition = Vector3.MoveTowards(pagePivot.localPosition, pos, Time.deltaTime *(speed/1000));
            pagePivot.localRotation = Quaternion.RotateTowards(pagePivot.localRotation, rot, Time.deltaTime * speed);
            yield return null;
        }
        pagePivot.localPosition = pos;
        pagePivot.localRotation = rot;
    }
}
