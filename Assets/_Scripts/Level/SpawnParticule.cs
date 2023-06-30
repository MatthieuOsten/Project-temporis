using UnityEngine;

public static class SpawnParticule 
{
    public static void PlayParticule(ParticleSystem particule)
    {
        particule.Play();
    }

    public static void StopParticule(ParticleSystem particule)
    {
        particule.Stop();
    }
}
