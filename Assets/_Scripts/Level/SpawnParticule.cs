using UnityEngine;

public class SpawnParticule : MonoBehaviour
{
    [SerializeField] ParticleSystem _particule;

    private void Start()
    {
        _particule.Stop();
    }

    public void Particule()
    {
        _particule.Play();
    }
}
