using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirrorPuzzle : MonoBehaviour
{
    [SerializeField] LineRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] LayerMask mirrorLayerMask;
    List<ReflectingMirror> _mirrors;
    [SerializeField] EditIllustrationButton[] _allButtons;

    // Start is called before the first frame update
    void Start()
    {
        _mirrors = new List<ReflectingMirror>();
        _ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        _lightRay.positionCount = 2;
        _lightRay.SetPosition(0, _rayOrigin.position);
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
                _lightRay.positionCount++;
                _ray = new Ray(hit.point, Vector3.Reflect(_ray.direction, hit.normal));
                _lightRay.SetPosition(_lightRay.positionCount - 1, _ray.origin + _ray.direction * 10);
                ReflectingMirror newMirror = hit.transform.GetComponentInParent<ReflectingMirror>();
                if (_mirrors.Count == 0)
                {
                    newMirror.rotModified += OnMirrorRotModified;
                    newMirror.reflectingMirrorPuzzle = this;
                    _mirrors.Add(newMirror);
                }
                else
                {
                    if (!_mirrors.Contains(newMirror))
                    {
                        newMirror.rotModified += OnMirrorRotModified;
                    }
                    if (_mirrors[_mirrors.Count - 1] != newMirror)
                    {
                        _mirrors.Add(newMirror);
                        newMirror.reflectingMirrorPuzzle = this;
                    }
                }
            }
            if(hit.transform.gameObject.layer == 13)
            {
                LightOrb lightOrb;
                if (hit.collider.TryGetComponent<LightOrb>(out lightOrb))
                {
                    lightOrb.ActivateOrb();
                    _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                }
            }
            else if (_lightRay.GetPosition(_lightRay.positionCount - 1) != hit.point)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
            }
        }
    }

    void OnMirrorRotModified(ReflectingMirror mirror)
    {
        int id = _mirrors.IndexOf(mirror);
        for (int i = _mirrors.Count - 1; i > id; i--)
        {
            ReflectingMirror lastMirror = _mirrors[i];
            _mirrors.RemoveAt(i);
            if (!_mirrors.Contains(lastMirror))
            {
                lastMirror.rotModified -= OnMirrorRotModified;
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
                _lightRay.positionCount++;
                _lightRay.SetPosition(_lightRay.positionCount - 1, _ray.origin + _ray.direction * 10);
            }
        }
        DrawRay();
    }
    public void LockAllMirrorsButtons()
    {
        for (int i = 0; i < _allButtons.Length; i++)
        {
            _allButtons[i].illustrationButton.interactable = false;
        }
    }

    public void UnlockAllMirrorsButtons()
    {
        for (int i = 0; i < _allButtons.Length; i++)
        {
            _allButtons[i].illustrationButton.interactable = true;
        }
    }
}
