using UnityEngine;

public class DamLever : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] GameObject _gameObjectToRotate;
    [SerializeField] private float _rotationAngle = 90f;
    [SerializeField] private float _rotationSpeed = 10f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OpenDam();
        }
    }

    public void OpenDam()
    {
        _gameObjectToRotate.transform.RotateAround(_point.position, Vector3.right, _rotationAngle);
    }
}
