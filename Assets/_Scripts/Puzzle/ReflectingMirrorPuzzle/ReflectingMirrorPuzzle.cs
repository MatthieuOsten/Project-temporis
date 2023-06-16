using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectingMirrorPuzzle : MonoBehaviour
{
    [SerializeField] TubeRenderer _lightRay;
    Ray _ray;
    [SerializeField] Transform _rayOrigin;
    [SerializeField] LayerMask mirrorLayerMask;
    bool _canDraw;
    int _mirrorToRot, _mirrorsReady;

    // Start is called before the first frame update
    void Start()
    {
        _canDraw = true;
        _ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
        _lightRay.positionCount = 2;
        _lightRay.SetPosition(0, _rayOrigin.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(_canDraw)
        {
            DrawRay();
        }
    }

    void DrawRay()
    {
        RaycastHit hit;
        Debug.DrawRay(_ray.origin, _ray.direction);
        if (Physics.Raycast(_ray.origin, _ray.direction, out hit, mirrorLayerMask))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
                _ray = new Ray(hit.point, Vector3.Reflect(_ray.direction, hit.normal));
                _lightRay.AddPosition(_ray.origin + _ray.direction * 100);
            }
            else if (_lightRay.GetPosition(_lightRay.positionCount - 1) != hit.point)
            {
                _lightRay.SetPosition(_lightRay.positionCount - 1, hit.point);
            }
        }
    }

    public void ResetRay()
    {
        _mirrorsReady++;
        if(_mirrorToRot==_mirrorsReady)
        {
            _ray = new Ray(_rayOrigin.position, _rayOrigin.forward);
            _lightRay.positionCount = 2;
            _lightRay.SetPosition(0, _rayOrigin.position);
            _lightRay.gameObject.SetActive(true);
            _canDraw = true;
            _mirrorsReady = 0;
            _mirrorToRot = 0;
        }
    }

    public void BlockRay()
    {
        _mirrorToRot++;
        _canDraw = false;
        _lightRay.gameObject.SetActive(false);
    }
}
