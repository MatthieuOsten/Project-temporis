using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirrorPuzzle : MonoBehaviour
{
    [SerializeField] TubeRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] LayerMask mirrorLayerMask;
    public List<ReflectingMirror> mirrors;

    // Start is called before the first frame update
    void Start()
    {
        _ray = new Ray(_rayOrigin.position, Vector3.forward);
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
                _ray = new Ray(hit.point, Vector3.Reflect(_ray.direction, hit.normal));
                _lightRay.AddPosition(_ray.origin + _ray.direction * 100);
                ReflectingMirror newMirror = hit.transform.GetComponentInParent<ReflectingMirror>();
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
            else if (_lightRay.GetPosition(_lightRay.positionCount - 1) != hit.point)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
            }
        }
    }

    void OnMirrorRotModified(ReflectingMirror mirror)
    {
        int id = mirrors.IndexOf(mirror);
        if (mirrors[mirrors.Count - 1] != mirror)
        {
            for (int i = mirrors.Count - 1; i > id; i--)
            {
                ReflectingMirror lastMirror = mirrors[i];
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
        DrawRay();
    }
}
