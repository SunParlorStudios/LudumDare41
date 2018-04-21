using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotParticle : MonoBehaviour
{
  void FixedUpdate()
  {
    if (GetComponent<ParticleSystem>().isPlaying == false)
    {
      Destroy(gameObject);
    }
  }
}
