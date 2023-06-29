using UnityEngine;

public class SpawnParticule : MonoBehaviour
{
    public void PlayParticule(ParticleSystem particule)
    {
        particule.Play();
    }

    public void StopParticule(ParticleSystem particule)
    {
        particule.Stop();
    }
}
