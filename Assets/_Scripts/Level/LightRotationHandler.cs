using UnityEngine;

public class LightRotationHandler : MonoBehaviour
{
    [SerializeField] Transform _light;
    [SerializeField] TimerBoucle _timer;

    private Quaternion _initilaRotation;  
    private Quaternion _targetRotation;
    private float _maxRotateTimer;
    private float _currentRotateTimer;


    private void Start()
    {
        _initilaRotation = _light.rotation;
        _targetRotation = _initilaRotation * Quaternion.Euler(-5, 0, 0);
        _maxRotateTimer = _timer.MaxTimer;
    }
    private void Update()
    {
        if (_currentRotateTimer < _maxRotateTimer)
        {
            float t = _currentRotateTimer / _maxRotateTimer;
            _light.rotation = Quaternion.Slerp(_initilaRotation, _targetRotation, t);
            _currentRotateTimer += Time.deltaTime;
            // Interpolation linéaire entre la rotation initiale et la rotation cible
        }
    }
}
