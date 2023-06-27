using UnityEngine;

public class CameraBlend : MonoBehaviour
{
    [SerializeField] Camera _playerCam;
    [SerializeField] Camera _templeCam;
    [SerializeField] Camera[] _othersCam;
    [SerializeField] float _transitionDuration = 2.0f;

    private float _transitionTimer;
    private bool _isTransitioning;

    public bool IsTransitioning
    { 
        get { return _isTransitioning;  } 
        set { _isTransitioning = value; }
    }

    private void TransitionBetweenCameras(Camera camToGo, Camera camToLeave)
    {
        camToGo.gameObject.SetActive(!camToGo.gameObject.activeInHierarchy);
        // Incrémenter le minuteur de transition
        _transitionTimer += Time.deltaTime;

        // Calculer le pourcentage de transition (entre 0 et 1)
        float t = Mathf.Clamp01(_transitionTimer / _transitionDuration);

        // Effectuer une transition linéaire entre les positions et rotations des caméras
        transform.position = Vector3.Lerp(camToLeave.transform.position, camToGo.transform.position, t);
        transform.rotation = Quaternion.Lerp(camToLeave.transform.rotation, camToGo.transform.rotation, t);

        // Si la transition est terminée, désactiver la caméra 1 et arrêter la transition
        if (_transitionTimer >= _transitionDuration)
        {
            camToLeave.gameObject.SetActive(!camToLeave.gameObject.activeInHierarchy);
            _isTransitioning = false;
        }

    }

    private Camera CheckWhichCamIsActiv()
    {
        foreach (Camera cam in _othersCam)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam;
            }
        }
        return null; 
    }

    public void StartTransition()
    {
        // Activer la caméra 2 et démarrer la transition
        TransitionBetweenCameras(_templeCam, _playerCam);
    }

    public void BackToPlayer()
    {
        // Activer la caméra 2 et démarrer la transition
        TransitionBetweenCameras(_playerCam, CheckWhichCamIsActiv());
    }
}
