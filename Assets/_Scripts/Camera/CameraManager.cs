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
    bool _playerLooking, _cameraLooking, _cameraLookingSeconds;

    private void Awake()
    {
        _instance = this;
        _playerLooking = false;
        _cameraLooking = false;
        _cameraLookingSeconds = false;
    }

    #region NOTE BOOK
    public void SetNoteBookView()
    {
        StartCoroutine(CameraLookToward(_noteBookPOV.localRotation, 2));
    }
    public void SetNoteBookMidView()
    {
        StartCoroutine(CameraLookToward(_noteBookMidPOV.localRotation, 2));
    }
    #endregion

    #region LOOK AT FUNCTIONS
    public void LookAt(Transform target, float speed)
    {
        Quaternion rot = Quaternion.Euler(_player.rotation.eulerAngles.x, Quaternion.LookRotation(target.position - _player.position).eulerAngles.y, _player.eulerAngles.z);
        StartCoroutine(LookToward(_player, rot, 200));
    }

    public void LookAtForSeconds(Transform target, float speed, float lookDuration)
    {
        Transform currentCamera = CameraUtility.Camera.transform;
        StartCoroutine(LookTowardForSeconds(currentCamera, Quaternion.LookRotation(target.position - currentCamera.position), speed, lookDuration));
    }

    IEnumerator LookToward(Transform target, Quaternion rot, float speed)
    {
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed /* Time.deltaTime*/);
            yield return null;
        }
        target.localRotation = rot;
    }
    IEnumerator CameraLookToward(Quaternion rot, float speed)
    {
        Transform target = CameraUtility.Camera.transform;
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed /* Time.deltaTime*/);
            yield return null;
        }
        target.localRotation = rot;
    }
    IEnumerator LookTowardForSeconds(Transform target, Quaternion rot, float speed, float lookDuration)
    {
        Quaternion initRot = target.rotation;
        while (target.localRotation != rot)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, rot, speed /* Time.deltaTime*/);
            yield return null;
        }
        target.localRotation = rot;
        yield return new WaitForSeconds(lookDuration);
        StartCoroutine(LookToward(target, initRot, speed));
    }
    #endregion
}
