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

    private SpawnParticule _particule;
    [SerializeField] private ParticleSystem _completedParticuleToSpawn;
    [SerializeField] private ParticleSystem[] _rotatingParticuleToSpawn;

    private PlayAudio _audio;
    [SerializeField] AudioSource _source;
    [SerializeField] private AudioClip _rotatingClip;

    [SerializeField] private rotateElement[] _tabInteract;
    [SerializeField] private bool _isFinish = false;
    [SerializeField] private EventDamScriptable _eventCompleted;

    public int ElementsCount { get { return _tabInteract.Length;} }

    private void Start()
    {
        _completedParticuleToSpawn.Stop();
        for (int i = 0; i < _rotatingParticuleToSpawn.Length; i++)
        {
            _rotatingParticuleToSpawn[i].Stop();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsCompleted() && !_isFinish)
        {
            _isFinish = true;
            _particule.PlayParticule(_completedParticuleToSpawn); // activ particule fin statue
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

            SetParticule(part);
            _audio.PlayClipAtPoint(_source, _rotatingClip);
            thePart.ChangeFace(face);
        }
        else
        {
            Debug.LogError("This part dont exist on this manager -> " + part);
        }

        for (int i = 0; i < _rotatingParticuleToSpawn.Length; i++)
        {
            _particule.StopParticule(_rotatingParticuleToSpawn[i]);
        }
        _audio.StopPlay(_source, _rotatingClip);
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

    private void SetParticule(int part)
    {
        switch(part)
        {
            case 0:
                _particule.PlayParticule(_rotatingParticuleToSpawn[0]);
                break;
            case 1:
                _particule.PlayParticule(_rotatingParticuleToSpawn[0]);
                _particule.PlayParticule(_rotatingParticuleToSpawn[1]);
                break;
            case 2:
                _particule.PlayParticule(_rotatingParticuleToSpawn[1]);
                _particule.PlayParticule(_rotatingParticuleToSpawn[2]);
                break;
        }
    }
}
