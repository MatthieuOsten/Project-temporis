using UnityEngine;

public class DamEvent : MonoBehaviour
{
    [SerializeField] EventDamScriptable _eventDam;
    [SerializeField] GameObject[] _water;
    [SerializeField] OpenTemple _openTemple;
    [SerializeField] private Transform _point;
    [SerializeField] GameObject _gameObjectToRotate;
    [SerializeField] private float _rotationAngle = 90f;
    [SerializeField] private float _rotationSpeed = 10f;

    private void Awake()
    {
        _eventDam.Event += OpenDam;
    }

    public void OpenDam(bool openDam)
    {
        _gameObjectToRotate.transform.RotateAround(_point.position, Vector3.right, _rotationAngle);
        for (int i = 0; i < _water.Length; i++)
        {
            _water[i].SetActive(!_water[i].activeInHierarchy);
        }
        _openTemple.OpenTempleDoor();
    }
}
