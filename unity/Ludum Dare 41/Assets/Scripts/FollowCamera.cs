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

    transform.position = Vector3.Lerp(transform.position, t, stepSize);
	}
}
