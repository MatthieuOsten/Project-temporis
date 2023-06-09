using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteBookPage : MonoBehaviour
{
    [SerializeField] GameObject frontCanvas, backCanvas;
    [SerializeField] Image frontIllustration, backIllustration;
    [SerializeField] TextMeshProUGUI frontDescription, backDescription;
    [SerializeField] Transform pagePivot;

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

    public void SetFrontPage(EntryInfoScriptable entryInfo)
    {
        frontCanvas.SetActive(true);
        frontIllustration.sprite = entryInfo.entryIllustration;
        frontDescription.text = entryInfo.entryDescription;
    }

    public void SetBackPage(EntryInfoScriptable entryInfo)
    {
        backCanvas.SetActive(true);
        backIllustration.sprite = entryInfo.entryIllustration;
        backDescription.text = entryInfo.entryDescription;
    }

    public void ShowFrontPage()
    {
        frontCanvas.SetActive(true);
    }

    public void ShowBackPage()
    {
        backCanvas.SetActive(true);
    }

    public void HideFrontPage()
    {
        frontCanvas.SetActive(false);
    }

    public void HideBackPage()
    {
        backCanvas.SetActive(false);
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
