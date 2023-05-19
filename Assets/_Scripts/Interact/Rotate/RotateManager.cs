using UnityEngine;

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
    [SerializeField] private EventDamScriptable _eventCompleted;

    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _isFinish = true;
            _eventCompleted.EventInvoke(_isFinish);
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
