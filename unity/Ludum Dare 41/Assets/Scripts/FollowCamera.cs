using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
  public Transform target;
  public Vector3 offset;

  [Range(0.0f, 1.0f)]
  public float stepSize;

  void Start()
  {

  }

  void FixedUpdate()
  {
    if (target == null)
    {
      return;
    }

    Vector3 t = target.position + offset;

    Rigidbody2D rb = null;
    if ((rb = target.GetComponent<Rigidbody2D>()) != null)
    {
      Vector3 v = rb.velocity;
      v.y = 0.0f;
      t += v;
    }

    transform.position = Vector3.Lerp(transform.position, t, stepSize);
  }
}
