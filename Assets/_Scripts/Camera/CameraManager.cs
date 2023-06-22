using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region SINGELTON
    static CameraManager _instance;
    static public CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject newCameraManager = new GameObject("CameraManager");
                _instance = newCameraManager.AddComponent<CameraManager>();
                return _instance;
            }
            else
            {
                return _instance;
            }
        }
        set
        {
            if (Instance != null)
            {
                Destroy(value.gameObject);
            }
        }
    }
    #endregion

    [SerializeField] Transform _noteBookPOV, _noteBookMidPOV;
    [SerializeField] Transform _player;
    Coroutine _setCameraView, _lookToward, _lookTowardForSeconds;

    private void Awake()
    {
        _instance = this;
    }

    #region NOTE BOOK
    public void SetNoteBookView()
    {
        if(_setCameraView != null)
        {
            StopCoroutine(_setCameraView);
        }
        _setCameraView = StartCoroutine(SetCameraView(_noteBookPOV.localRotation, 50));
    }
    public void SetNoteBookMidView()
    {
        if (_setCameraView != null)
        {
            StopCoroutine(_setCameraView);
        }
        _setCameraView = StartCoroutine(SetCameraView(_noteBookMidPOV.localRotation, 50));
    }
    #endregion

    #region LOOK AT FUNCTIONS
    public void LookAt(Transform target, float speed)
    {
        if(_lookToward != null)
        {
            StopCoroutine(_lookToward);
        }
        Quaternion rot = Quaternion.Euler(_player.rotation.eulerAngles.x, Quaternion.LookRotation(target.position - _player.position).eulerAngles.y, _player.eulerAngles.z);
        _lookToward = StartCoroutine(LookToward(_player, rot, 200));
    }

    public void LookAtForSeconds(Transform target, float speed, float lookDuration)
    {
        if(_lookTowardForSeconds != null)
        {
            StopCoroutine(_lookTowardForSeconds);
        }
        Transform currentCamera = CameraUtility.Camera.transform;
        _lookTowardForSeconds = StartCoroutine(LookTowardForSeconds(currentCamera, Quaternion.LookRotation(target.position - currentCamera.position), speed, lookDuration));
    }
    #endregion

    #region COROUTINES
    IEnumerator LookToward(Transform target, Quaternion rot, float speed)
    {
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed * Time.deltaTime);
            yield return null;
        }
        target.localRotation = rot;
        _lookToward = null;
    }
    IEnumerator SetCameraView(Quaternion rot, float speed)
    {
        InputManager.Instance.closeNoteBookEnabled = false;
        Transform target = CameraUtility.Camera.transform;
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed * Time.deltaTime);
            yield return null;
        }
        target.localRotation = rot;
        _setCameraView = null;
        InputManager.Instance.closeNoteBookEnabled = true;
    }
    IEnumerator LookTowardForSeconds(Transform target, Quaternion rot, float speed, float lookDuration)
    {
        InputManager.Instance.DisableAllInGameActions();
        Quaternion initRot = target.rotation;
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed * Time.deltaTime);
            yield return null;
        }
        target.localRotation = rot;
        yield return new WaitForSeconds(lookDuration);
        yield return StartCoroutine(LookToward(target, initRot, speed));
        _lookTowardForSeconds = null;
        InputManager.Instance.EnableAllInGameActions();
    }
    #endregion
}
