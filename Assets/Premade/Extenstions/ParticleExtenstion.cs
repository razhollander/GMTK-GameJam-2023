using UnityEngine;

public static class ParticleExtenstion
{
    public static float GetParticleAge(this ParticleSystem.Particle particle)
    {
        return particle.startLifetime - particle.remainingLifetime;
    }
}