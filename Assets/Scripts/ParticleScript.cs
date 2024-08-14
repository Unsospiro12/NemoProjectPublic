using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem startParticle;
    [SerializeField] ParticleSystem OnGoingParticle;

    private void OnEnable()
    {
        startParticle.Play();
        OnGoingParticle.Play();
    }

    private void OnDisable()
    {
        startParticle.Stop();
        OnGoingParticle.Stop();
    }
}
