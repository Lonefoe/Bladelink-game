using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    public ParticleEffect[] particles;

    public void SpawnParticles(string name, Vector3 pos, bool randomRot = default)
    {
        ParticleEffect p = Array.Find(particles, particle => particle.name == name);
        if (p == null)
        {
            Debug.Log("Particles: " + name + " not found!");
            return;
        }

        var _particle = Instantiate(p.particlesPrefab, pos, Quaternion.identity) as GameObject;

        if (randomRot) _particle.transform.eulerAngles = new Vector3(_particle.transform.eulerAngles.x, _particle.transform.eulerAngles.y, UnityEngine.Random.Range(0f, 360f));
    }

}
