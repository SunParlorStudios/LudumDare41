using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinge : MonoBehaviour
{
  public Transform followPoint;

  private float restingDistance_;
  private float startAngle_;
  private Vector3 localOffset_;
  private Vector3 oldPoint_;

  void Start()
  {
    localOffset_ = followPoint.position - transform.position;
    restingDistance_ = localOffset_.magnitude;

    startAngle_ = transform.rotation.eulerAngles.x;
  }

  void FixedUpdate()
  {
    Vector3 d = followPoint.position - transform.position;
    Vector3 n = followPoint.TransformDirection(localOffset_).normalized;

    float dist = Mathf.Lerp(d.magnitude, restingDistance_, 0.5f);

    transform.position = followPoint.position - n * dist;
  }
}
