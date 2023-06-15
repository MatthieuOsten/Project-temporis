using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightPuzzle : MonoBehaviour
{
    [SerializeField] TubeRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] float maxLength = 100;
    //float remainingLength;
    [SerializeField] LayerMask mirrorLayerMask;
    public List<LightReflector> mirrors;
    [SerializeField] GameObject _laserPOVCamera;
    [SerializeField] GameUI _gameUI;

    // Start is called before the first frame update
    void Start()
    {
        //_laserPOVCamera.SetActive(false);
        _ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        _lightRay.positionCount = 2;
        _lightRay.SetPosition(0, _rayOrigin.position);
        _gameUI.laserPOVCameraShowed += ShowLaserPOVCamera;
        _gameUI.laserPOVCameraHidded += HideLaserPOVCamera;
    }

    // Update is called once per frame
    void Update()
    {
        DrawRay();
    }

    void DrawRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                _ray = new Ray(hit.point, Vector3.Reflect(_ray.direction, hit.normal));
                _lightRay.AddPosition(_ray.origin + _ray.direction * 100);
                LightReflector newMirror = hit.transform.GetComponentInParent<LightReflector>();
                if (mirrors.Count == 0)
                {
                    newMirror.rotModified += OnMirrorRotModified;
                    mirrors.Add(newMirror);
                }
                else
                {
                    if (!mirrors.Contains(newMirror))
                    {
                        newMirror.rotModified += OnMirrorRotModified;
                    }
                    if (mirrors[mirrors.Count - 1] != newMirror)
                    {
                        mirrors.Add(newMirror);
                    }
                }
            }
            else if(_lightRay.GetPosition(_lightRay.positionCount - 1) != hit.point)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
            }
        }
    }

    void OnMirrorRotModified(LightReflector mirror)
    {
        int id = mirrors.IndexOf(mirror);
        if (mirrors[mirrors.Count-1] != mirror)
        {
            for (int i = mirrors.Count - 1; i > id; i--)
            {
                LightReflector lastMirror = mirrors[i];
                mirrors.RemoveAt(i);
                if (!mirrors.Contains(lastMirror))
                {
                    lastMirror.rotModified -= OnMirrorRotModified;
                }
            }
        }
        _lightRay.positionCount = id + 2;
        Vector3 directionPoint = _lightRay.GetPosition(id + 1);
        Vector3 lastPoint = _lightRay.GetPosition(id);
        _ray = new Ray(lastPoint, new Vector3(directionPoint.x - lastPoint.x, directionPoint.y - lastPoint.y, directionPoint.z - lastPoint.z));
        RaycastHit hit;
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _ray = new Ray(hit.point, Vector3.Reflect(_ray.direction, hit.normal));
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                _lightRay.AddPosition(_ray.origin + _ray.direction * 100);
            }
        }
        SetLaserPOVCamera(_ray.origin, _ray.direction, 0.2f);
        DrawRay();
    }

    void SetLaserPOVCamera(Vector3 pos, Vector3 direction, float yPosOffset)
    {
        _laserPOVCamera.transform.position = pos;
        _laserPOVCamera.transform.forward = direction;
        _laserPOVCamera.transform.position += _laserPOVCamera.transform.up * yPosOffset;
    }

    void ShowLaserPOVCamera()
    {
        _laserPOVCamera.SetActive(true);
    }
    void HideLaserPOVCamera()
    {
        _laserPOVCamera.SetActive(false);
    }
}
