using UnityEngine;

public class TriggerEventLevelBird : MonoBehaviour
{
    [SerializeField] EventTriggerLevel _triggerEvent;

    private void Awake()
    {
        _triggerEvent.Event += TriggerEventTest;
    }

    private void TriggerEventTest(Collider obj)
    {
        Debug.Log("son joue");
        AudioSource.PlayClipAtPoint(_triggerEvent._audio, gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _triggerEvent.EventInvoke(other);
            Destroy(gameObject);
        }
    }
}
