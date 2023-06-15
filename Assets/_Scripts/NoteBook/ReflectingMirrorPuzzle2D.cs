using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirrorPuzzle2D : MonoBehaviour
{
    [SerializeField] LineRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] LayerMask mirrorLayerMask;
    public List<ReflectingMirror> mirrors;

    // Start is called before the first frame update
    void Start()
    {
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
                Transform target = hit.collider.transform;
                _lightRay.SetPosition(_lightRay.positionCount - 1, target.position);
                _lightRay.positionCount++;
                _ray = new Ray(target.position, Vector2.Reflect(_ray.direction, target.up));
                _lightRay.SetPosition(_lightRay.positionCount - 1, target.position + _ray.direction * 100);
                ReflectingMirror newMirror = target.GetComponent<ReflectingMirror>();
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
            else
            {
                Debug.Log("Why?");
            }
        }
    }

    void OnMirrorRotModified(ReflectingMirror mirror)
    {
        int id = mirrors.IndexOf(mirror);
        Debug.Log(id);
        for (int i = mirrors.Count - 1; i > id; i--)
        {
            ReflectingMirror lastMirror = mirrors[i];
            mirrors.RemoveAt(i);
            if (!mirrors.Contains(lastMirror))
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
                Transform target = hit.collider.transform;
                _ray = new Ray(target.position, Vector2.Reflect(_ray.direction, target.up));
                _lightRay.SetPosition(_lightRay.positionCount - 1, target.position);
                _lightRay.positionCount++;
                _lightRay.SetPosition(_lightRay.positionCount - 1, target.position + _ray.direction * 100);
            }
        }
        DrawRay();
    }
}
