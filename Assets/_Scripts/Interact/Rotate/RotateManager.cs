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

    public int ElementsCount { get { return _tabInteract.Length;} }

    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _isFinish = true;
            _eventCompleted.EventInvoke(_isFinish);
        }
    }

    public void RotatePartTo(int part, int face)
    {
        if (part < ElementsCount && part >= 0) {
            InteractRotate thePart = _tabInteract[part].Element;

            if (face < thePart.Faces && face >= 1)
            {
                Debug.Log("This face " + face + " is set on this part " + part);
            }
            else
            {
                Debug.LogWarning("This face " + face + " is inexist on this part " + part);
            }

            thePart.ActualFace = face;
        }
        else
        {
            Debug.LogError("This part dont exist on this manager -> " + part);
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
