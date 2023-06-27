using UnityEngine;

public class LightRotationHandler : MonoBehaviour
{
    [SerializeField] Transform _light;
    [SerializeField] Vector3 _axe;
    [SerializeField] TimerBoucle _timer;

    private Quaternion _initialRotation;
    private Quaternion _targetRotation;
    private float _maxRotateTimer;
    private float _currentRotateTimer;

    private void Start()
    {
        _initialRotation = _light.rotation;
        _maxRotateTimer = _timer.MaxTimer;
        _currentRotateTimer = _timer.CurrentTimer;
        _targetRotation = Quaternion.AngleAxis(0f, _axe); // Rotation de -205 degrés
    }

    private void Update()
    {
        _currentRotateTimer += Time.deltaTime;

        if (_currentRotateTimer <= _maxRotateTimer)
        {
            // Interpolation linéaire entre la rotation initiale et la rotation cible
            float t = _currentRotateTimer / _maxRotateTimer;
            _light.rotation = Quaternion.Lerp(_initialRotation, _targetRotation, t);
        }
    }
}
