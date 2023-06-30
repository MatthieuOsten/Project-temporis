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

    [SerializeField] private ParticleSystem _completedParticuleToSpawn;
    [SerializeField] private ParticleSystem[] _rotatingParticule;

    [SerializeField] AudioClip _clip;
    [SerializeField] AudioSource _source;

    [SerializeField] private bool _isFinish = false;
    [SerializeField] private EventDamScriptable _eventCompleted;

    public int ElementsCount { get { return _tabInteract.Length; } }


    private void Start()
    {
        StopParticule();
        SpawnParticule.StopParticule(_completedParticuleToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _isFinish = true;
            SpawnParticule.PlayParticule(_completedParticuleToSpawn); // activ particule fin statue
            _eventCompleted.EventInvoke(_isFinish);
        }
    }

    public void RotatePartTo(int part, int face)
    {
        if (part < ElementsCount && part >= 0)
        {
            InteractRotate thePart = _tabInteract[part].Element;

            if (face < thePart.Faces && face >= 1)
            {
                Debug.Log("This face " + face + " is set on this part " + part);
            }
            else
            {
                Debug.LogWarning("This face " + face + " is inexist on this part " + part);
            }
            
            PlayParticule(part);
            PlayAudio.PlayClip(_source, _clip);
            thePart.ChangeFace(face);
        }
        else
        {
            Debug.LogError("This part dont exist on this manager -> " + part);
        }

        PlayAudio.StopPlay(_source);
        StopParticule();
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

    private void PlayParticule(int index)
    {
        switch (index)
        {
            case 0:
                SpawnParticule.PlayParticule(_rotatingParticule[0]);
                break;

            case 1:
                SpawnParticule.PlayParticule(_rotatingParticule[0]);
                SpawnParticule.PlayParticule(_rotatingParticule[1]);
                break;

            case 2:
                SpawnParticule.PlayParticule(_rotatingParticule[1]);
                SpawnParticule.PlayParticule(_rotatingParticule[2]);
                break;
        }
    }

    private void StopParticule()
    {
        for (int i = 0; i < _rotatingParticule.Length; i++)
        {
            SpawnParticule.StopParticule(_rotatingParticule[i]);
        }
    }
}
