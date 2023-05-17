using UnityEngine;

public class DamEvent : MonoBehaviour
{
    [SerializeField] EventDamScriptable _eventDam;
    [SerializeField] OpenTemple _openTemple;
    [SerializeField] private Transform _point;
    [SerializeField] GameObject _gameObjectToRotate;
    [SerializeField] private float _rotationAngle = 90f;
    [SerializeField] private float _rotationSpeed = 10f;

    private void Awake()
    {
        _eventDam.Event += OpenDam;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _eventDam._completed = true;
        }
    }

    public void OpenDam(bool openDam)
    {
        _eventDam._completed = openDam;
        _gameObjectToRotate.transform.RotateAround(_point.position, Vector3.right, _rotationAngle);
        _openTemple.OpenTempleDoor();
    }
}
