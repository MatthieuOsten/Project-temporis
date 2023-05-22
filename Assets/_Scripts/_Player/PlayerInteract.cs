using System;
using System.Collections;
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
    private Interactive obj;
    private Transform _mirrorInfo;

    bool canDetect = true;

    [Header("DEBUG")]
    [SerializeField] private bool _debugRaycastCollider;

    // Update is called once per frame
    void Update()
    {
        if(canDetect)
        {
            // create a ray at the center of the camera shooting outwards
            Ray ray = new Ray(CameraUtility.Camera.transform.position, CameraUtility.Camera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * _rayDistance);
            RaycastHit hitInfo; // var to store our coll info.
            if (Physics.Raycast(ray, out hitInfo, _rayDistance, _layer))
            {
                if (_debugRaycastCollider) { Debug.Log("see object with layer " + hitInfo.transform.gameObject.layer + " -> " + hitInfo.transform.name); }

                if (hitInfo.transform.gameObject.layer == 6)
                {

                    if (/*hitInfo.collider.GetComponent<Engraving>() != null*/ hitInfo.collider.TryGetComponent<Engraving>(out _engravingInfo))
                    {
                        /*_engravingInfo = hitInfo.collider.GetComponent<Engraving>(); // var to store engraving info*/
                        if (!_engravingInfo.EngravingScriptable.HasBeenStudied)
                        {
                            _gameUI.ShowInteractText("Press E to interact");
                            InputManager.Instance.InteractStarted = TranslatePrint;
                        }
                    }
                }
                else if (hitInfo.transform.gameObject.layer == 8)
                {

                    _gameUI.ShowInteractText("Press E to grab");
                    InputManager.Instance.InteractStarted = GrabMirror;
                    InputManager.Instance.InteractCancelled = LetOffMirror;
                    _mirrorInfo = hitInfo.transform;
                }
                else if (hitInfo.transform.gameObject.layer == 9)
                {

                    if (hitInfo.collider.TryGetComponent<Interactive>(out obj)) {
                        _gameUI.ShowInteractText("Press E to interact");

                        InputManager.Instance.InteractStarted = obj.StartedUse;
                        InputManager.Instance.InteractCancelled = obj.CancelledUse;
                    }

                }
            }
            else
            {
                _gameUI.HideInteractText();
                InputManager.Instance.InteractStarted = null;
                InputManager.Instance.InteractCancelled = null;
            }
        }
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

    public void GrabMirror(InputAction.CallbackContext context)
    {
        canDetect = false;
        _gameUI.HideInteractText();
        StartCoroutine(LookToward((new Vector3(_mirrorInfo.position.x, transform.position.y, _mirrorInfo.position.z) + _mirrorInfo.forward * -0.8f), _mirrorInfo.rotation));
        InputManager.Instance.DisableActions(true, false, true, false);
        LightReflector parent = _mirrorInfo.GetComponentInParent<LightReflector>();
        InputManager.Instance.MoveChanged = parent.RotateReflector; 
        transform.parent = parent.transform;
        parent.direction = int.Parse(_mirrorInfo.name);
    }
    public void LetOffMirror(InputAction.CallbackContext context)
    {
        transform.parent = null;
        InputManager.Instance.EnableActions(false, false, true, false);
        InputManager.Instance.MoveChanged = GetComponent<PlayerMovement>().OnMove;
        _mirrorInfo.GetComponentInParent<LightReflector>().Reset();
        canDetect = true;
    }

    IEnumerator LookToward(Vector3 pos, Quaternion rot)
    {
        while ((transform.position != pos) || (transform.rotation != rot))
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, 2f * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 80f * Time.deltaTime);
            yield return null;
        }
        InputManager.Instance.EnableActions(true, false, false, false);
    }
}
