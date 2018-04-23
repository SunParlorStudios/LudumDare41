using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitSound : MonoBehaviour
{
  AudioSource[] sources_;

  void Start()
  {
    sources_ = GetComponents<AudioSource>();
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    foreach (AudioSource s in sources_)
    {
      s.Play();
    }
  }
}
