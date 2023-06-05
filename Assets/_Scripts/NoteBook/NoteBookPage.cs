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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipPage(float posZ, float rotY)
    {
        pagePivot.localPosition = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        pagePivot.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
    }

    public void FlipPage(float posZ, float rotY, float speed)
    {
        Debug.Log("Flip");
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, 0));
    }

    public void FlipPage(float posZ, float rotY, float speed, float delay)
    {
        Debug.Log("Flip");
        Vector3 pos = new Vector3(pagePivot.localPosition.x, pagePivot.localPosition.y, posZ);
        Quaternion rot = Quaternion.Euler(transform.localRotation.eulerAngles.x, rotY, transform.localRotation.eulerAngles.z);
        StartCoroutine(FlipPivot(pos, rot, speed, delay));
    }

    public void SetFrontPage(EngravingScriptable pageInfo)
    {
        frontCanvas.SetActive(true);
        frontIllustration.sprite = pageInfo.engravingSprite;
        frontDescription.text = pageInfo.engravingTranslate;
    }

    public void SetBackPage(EngravingScriptable pageInfo)
    {
        backCanvas.SetActive(true);
        backIllustration.sprite = pageInfo.engravingSprite;
        backDescription.text = pageInfo.engravingTranslate;
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
    }
}
