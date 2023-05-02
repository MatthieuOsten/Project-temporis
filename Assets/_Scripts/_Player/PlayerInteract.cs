using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] GameUI _gameUI;
    [SerializeField] EngravingUI _noteBook;
    [SerializeField] PageList _pageInventory;
    [SerializeField] private float _rayDistance = 10f;
    [SerializeField] private LayerMask _layer;

    private Engraving _engravingInfo;

    // Update is called once per frame
    void Update()
    {
        // create a ray at the center of the camera shooting outwards
        Ray ray = new Ray(CameraUtility.Camera.transform.position, CameraUtility.Camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _rayDistance);
        RaycastHit hitInfo; // var to store our coll info.
        if (Physics.Raycast(ray, out hitInfo, _rayDistance, _layer))
        {
            if(hitInfo.transform.gameObject.layer == 6)
            {
                if (/*hitInfo.collider.GetComponent<Engraving>() != null*/ hitInfo.collider.TryGetComponent<Engraving>(out _engravingInfo))
                {
                    /*_engravingInfo = hitInfo.collider.GetComponent<Engraving>(); // var to store engraving info*/
                    if (!_engravingInfo.EngravingScriptable.HasBeenStudied)
                    {
                        _gameUI.SetInteractText(true);
                        InputManager.Instance.InteractStarted += TranslatePrint;
                    }
                }
            }
        }
        else
            _gameUI.SetInteractText(false);
    }

    public void TranslatePrint(InputAction.CallbackContext context)
    {
        /*if (context.ReadValueAsButton())
        {
            // create a ray at the center of the camera shooting outwards
            Ray ray = new Ray(CameraUtility.Camera.transform.position, CameraUtility.Camera.transform.forward);
            RaycastHit hitInfo; // var to store our coll info.
            if (Physics.Raycast(ray, out hitInfo, _rayDistance, _layer))
            {
                if (hitInfo.collider.GetComponent<Engraving>() != null)
                {
                    Engraving engravingToAdd = hitInfo.collider.GetComponent<Engraving>(); // var to store engraving info
                    EngravingScriptable engravingScriptableToAdd = engravingToAdd.EngravingScriptable;
                    if (!engravingScriptableToAdd.hasBeenStudied)
                    {
                        _noteBook.Set(engravingScriptableToAdd);
                        engravingToAdd.EngravingScriptable.HasBeenStudied = true;
                        _pageInventory.SetPageInfo(engravingScriptableToAdd); //ajoute la page à la liste
                    }
                }
            }
        }*/

        EngravingScriptable engravingScriptableToAdd = _engravingInfo.EngravingScriptable;
        _noteBook.Set(engravingScriptableToAdd);
        _engravingInfo.EngravingScriptable.HasBeenStudied = true;
        _pageInventory.SetPageInfo(engravingScriptableToAdd); //ajoute la page à la liste
        InputManager.Instance.InteractStarted -= TranslatePrint;
    }
}
