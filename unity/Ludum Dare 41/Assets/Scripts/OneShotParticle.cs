using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotParticle : MonoBehaviour
{
  void FixedUpdate()
  {
    ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

    foreach (ParticleSystem system in systems)
    {
      if (system.isPlaying == true)
      {
        return;
      }
    }

    ParticleSystem p = GetComponent<ParticleSystem>();
    if (p != null && p.isPlaying == true)
    {
      return;
    }

    Destroy(gameObject);
  }
}
