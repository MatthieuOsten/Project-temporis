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
        // Incr�menter le minuteur de transition
        _transitionTimer += Time.deltaTime;

        // Calculer le pourcentage de transition (entre 0 et 1)
        float t = Mathf.Clamp01(_transitionTimer / _transitionDuration);

        // Effectuer une transition lin�aire entre les positions et rotations des cam�ras
        transform.position = Vector3.Lerp(camToLeave.transform.position, camToGo.transform.position, t);
        transform.rotation = Quaternion.Lerp(camToLeave.transform.rotation, camToGo.transform.rotation, t);

        // Si la transition est termin�e, d�sactiver la cam�ra 1 et arr�ter la transition
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
        // Activer la cam�ra 2 et d�marrer la transition
        TransitionBetweenCameras(_templeCam, _playerCam);
    }

    public void BackToPlayer()
    {
        // Activer la cam�ra 2 et d�marrer la transition
        TransitionBetweenCameras(_playerCam, CheckWhichCamIsActiv());
    }
}
