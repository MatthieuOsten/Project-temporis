using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPuzzle : MonoBehaviour
{
    [SerializeField] LineRenderer _lightRayL, _lightRayR;
    Ray _rayL, _rayR;
    [SerializeField] Transform rayOriginL, rayOriginR;
    [SerializeField] float maxLength = 100;
    //float remainingLength;
    [SerializeField] LayerMask mirrorLayerMask;
    public List<LightReflector> mirrorsL, mirrorsR;

    // Start is called before the first frame update
    void Start()
    {
        _lightRayL.positionCount = 1;
        //_lightRayR.positionCount = 1;
        _lightRayL.SetPosition(0, rayOriginL.position);
        //_lightRayR.SetPosition(0, rayOriginR.position);
        _rayL = new Ray(rayOriginL.position, rayOriginL.forward);
        //_rayR = new Ray(rayOriginR.position, rayOriginR.forward);
    }

    // Update is called once per frame
    void Update()
    {
        DrawRayLeft();
        //DrawRayRight();
    }

    void DrawRayLeft()
    {
        RaycastHit hit;
        if (Physics.Raycast(_rayL.origin, _rayL.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _lightRayL.positionCount++;
                _lightRayL.SetPosition(_lightRayL.positionCount - 1, hit.point);
                //remainingLength -= Vector3.Distance(_ray.origin, hit.point);
                _rayL = new Ray(hit.point, Vector3.Reflect(_rayL.direction, hit.normal));
                LightReflector newMirror = hit.transform.GetComponentInParent<LightReflector>();
                if (!mirrorsL.Contains(newMirror))
                {
                    newMirror.rotModified += OnMirrorRotModifiedL;
                    mirrorsL.Add(newMirror);
                }
            }
            else if(_lightRayL.GetPosition(_lightRayL.positionCount - 1) != hit.point)
            {
                _lightRayL.positionCount++;
                _lightRayL.SetPosition(_lightRayL.positionCount - 1, hit.point);
            }
        }
        else
        {
            if (!(_lightRayL.GetPosition(_lightRayL.positionCount - 1) == _rayL.origin + _rayL.direction * 100))
            {
                _lightRayL.positionCount++;
                _lightRayL.SetPosition(_lightRayL.positionCount - 1, _rayL.origin + _rayL.direction * 100);
            }
        }
    }

    void DrawRayRight()
    {
        RaycastHit hit;
        if (Physics.Raycast(_rayR.origin, _rayR.direction, out hit))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _lightRayR.positionCount++;
                _lightRayR.SetPosition(_lightRayR.positionCount - 1, hit.point);
                //remainingLength -= Vector3.Distance(_ray.origin, hit.point);
                _rayR = new Ray(hit.point, Vector3.Reflect(_rayR.direction, hit.normal));
                LightReflector newMirror = hit.transform.GetComponentInParent<LightReflector>();
                if (!mirrorsR.Contains(newMirror))
                {
                    newMirror.rotModified += OnMirrorRotModifiedR;
                    mirrorsR.Add(newMirror);
                }
            }
            else if(_lightRayR.GetPosition(_lightRayR.positionCount - 1) != hit.point)
            {
                _lightRayR.positionCount++;
                _lightRayR.SetPosition(_lightRayR.positionCount - 1, hit.point);
            }
        }
        else
        {
            if (!(_lightRayR.GetPosition(_lightRayR.positionCount - 1) == _rayR.origin + _rayR.direction * 100))
            {
                _lightRayR.positionCount++;
                _lightRayR.SetPosition(_lightRayR.positionCount - 1, _rayR.origin + _rayR.direction * 100);
            }
        }
    }

    void OnMirrorRotModifiedL(LightReflector mirror)
    {
        int id = mirrorsL.IndexOf(mirror);
        for(int i = mirrorsL.Count - 1; i > id; i--)
        {
            mirrorsL[i].rotModified -= OnMirrorRotModifiedL;
            mirrorsL.RemoveAt(i);
        }
        Vector3 directionPoint = _lightRayL.GetPosition(id+1);
        _lightRayL.positionCount = id+1;
        Vector3 lastPoint = _lightRayL.GetPosition(id);
        _rayL = new Ray(lastPoint, new Vector3(directionPoint.x - lastPoint.x, directionPoint.y - lastPoint.y, directionPoint.z - lastPoint.z));
    }

    void OnMirrorRotModifiedR(LightReflector mirror)
    {
        int id = mirrorsR.IndexOf(mirror);
        for (int i = mirrorsR.Count - 1; i > id; i--)
        {
            mirrorsR[i].rotModified -= OnMirrorRotModifiedR;
            mirrorsR.RemoveAt(i);
        }
        Vector3 directionPoint = _lightRayR.GetPosition(id+1);
        _lightRayR.positionCount = id+1;
        Vector3 lastPoint = _lightRayR.GetPosition(id);
        _rayR = new Ray(lastPoint, new Vector3(directionPoint.x - lastPoint.x, directionPoint.y - lastPoint.y, directionPoint.z - lastPoint.z));
    }
}
