using UnityEngine;

public class TriggerEventNoteBook : MonoBehaviour
{
    [SerializeField] EventTriggerScriptableEnviroNoteBook _triggerEvent;
    [SerializeField] EngravingUI _noteBook;
    [SerializeField] PageList _pages;

    private void Awake()
    {
        _triggerEvent.Event += TriggerEventTest;
    }

    private void TriggerEventTest(Collider obj)
    {
        //Set les infos dans le noteBook
        //_noteBook.Set(_triggerEvent);
        _pages.SetPageInfo(_triggerEvent);
        Debug.Log(obj.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _triggerEvent._activate = true;
            //TriggerEventTest(other);
        }
    }
}
