using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateManager : MonoBehaviour
{
    [System.Serializable]
    struct rotateElement
    {
        [SerializeField] private InteractRotate _element;
        [SerializeField, Min(1)] private int _objectifFace;

        public InteractRotate Element { get { return _element; } }
        public int objectifFace { get { return _objectifFace; } }
    };

    [SerializeField] private rotateElement[] _tabInteract;
    [SerializeField] private bool _isFinish = false;
    [SerializeField] private UnityEvent _eventCompleted = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _eventCompleted.Invoke();
            _isFinish = true;
        }
    }

    private bool IsCompleted()
    {

        foreach (var item in _tabInteract)
        {
            if (item.objectifFace != item.Element.ActualFace)
            {
                return false;
            }
        }

        return true;
    }
}
