using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirrorPuzzle2D : MonoBehaviour
{
    [SerializeField] LineRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] LayerMask mirrorLayerMask;
    public List<ReflectingMirror2D> _mirrors;

    // Start is called before the first frame update
    void Start()
    {
        _mirrors = new List<ReflectingMirror2D>();
        _ray = new Ray(_rayOrigin.position, _rayOrigin.right);
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
        Debug.DrawRay(_ray.origin, _ray.direction, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                _lightRay.positionCount++;
                _ray = new Ray(hit.point, Vector2.Reflect(_ray.direction, hit.normal));
                _lightRay.SetPosition(_lightRay.positionCount - 1, _ray.origin + _ray.direction * 100);
                ReflectingMirror2D newMirror = hit.transform.GetComponent<ReflectingMirror2D>();
                if (_mirrors.Count == 0)
                {
                    newMirror.rotModified += OnMirrorRotModified;
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
                    }
                }
            }
            else if (_lightRay.GetPosition(_lightRay.positionCount - 1) != hit.point)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
            }
        }
    }

    public void OnMirrorRotModified(ReflectingMirror2D mirror)
    {
        int id = _mirrors.IndexOf(mirror);
        for (int i = _mirrors.Count - 1; i > id; i--)
        {
            ReflectingMirror2D lastMirror = _mirrors[i];
            _mirrors.RemoveAt(i);
            if (!_mirrors.Contains(lastMirror))
            {
                lastMirror.rotModified -= OnMirrorRotModified;
            }
        }
        _lightRay.positionCount = id + 2;
        Vector2 directionPoint = _lightRay.GetPosition(id + 1);
        Vector2 lastPoint = _lightRay.GetPosition(id);
        _ray = new Ray(lastPoint, new Vector3(directionPoint.x - lastPoint.x, directionPoint.y - lastPoint.y));
        RaycastHit hit;
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _ray = new Ray(hit.point, Vector2.Reflect(_ray.direction, hit.normal));
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                _lightRay.positionCount++;
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point + _ray.direction * 100);
            }
        }
        DrawRay();
    }
}
