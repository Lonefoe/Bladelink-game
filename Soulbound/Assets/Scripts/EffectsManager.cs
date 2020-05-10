using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    public ParticleEffect[] particles;

    public void SpawnParticles(string name, Vector3 pos)
    {
        ParticleEffect p = Array.Find(particles, particle => particle.name == name);
        if (p == null)
        {
            Debug.Log("Particles: " + name + " not found!");
            return;
        }

        Instantiate(p.particlesPrefab, pos, Quaternion.identity);
    }

}
