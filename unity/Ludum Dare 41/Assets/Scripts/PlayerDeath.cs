using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
  public float force;
  public float collisionRadius;

  void Start()
  {
    for (int i = 0; i < transform.childCount; ++i)
    {
      GameObject go = transform.GetChild(i).gameObject;

      Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
      if (rb == null)
      {
        continue;
      }

      Vector3 f = new Vector3(
        Random.Range(-force, force),
        Random.Range(0.0f, force * 2.0f),
        0.0f);

      rb.AddForce(f);
    }
  }
}
