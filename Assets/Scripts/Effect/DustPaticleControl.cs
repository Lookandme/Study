using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustPaticleControl : MonoBehaviour
{
    [SerializeField] private bool createDustOnWalk = true;
    [SerializeField] ParticleSystem dustParticleSystem;
    
    public void CreateDustParticles()
    {
        if (createDustOnWalk)
        {
            dustParticleSystem.Stop();
            dustParticleSystem.Play();
        }
    }
}
