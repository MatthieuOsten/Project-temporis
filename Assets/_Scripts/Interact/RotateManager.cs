using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateManager : MonoBehaviour
{

    struct rotateElement
    {
        private InteractRotate _element;
        private int _objectifFace;

        public InteractRotate Element { get { return _element; } }
        public int objectifFace { get { return _objectifFace; } }
    };

    private rotateElement[] _tabInteract;
    private bool _isFinish = false;
    private UnityEvent _eventCompleted = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _eventCompleted.Invoke();
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
