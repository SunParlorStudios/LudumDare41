using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Electricity : MonoBehaviour
{
  public Transform pointA;
  public Transform pointB;
  
  [Range(0.01f, 1.0f)]
  public float stepRatio;

  public float width;

  private LineRenderer renderer_;
  private Vector3[] points_;

	void Start()
  {
    renderer_ = GetComponent<LineRenderer>();
	}

  void Reconstruct()
  {
    Vector3 p1 = pointA.position;
    Vector3 p2 = pointB.position;

    Vector3 d = p2 - p1;

    Vector3 n1 = d.normalized;
    Vector3 n2 = new Vector3(d.y, -d.x, 0.0f);

    int numVertices = (int)(1.0f / stepRatio + 0.5f);

    if (points_ == null || numVertices != points_.Length)
    {
      points_ = new Vector3[numVertices];
    }

    for (int i = 0; i < numVertices; ++i)
    {
      float h = Random.Range(-width * 0.5f, width * 0.5f);
      float r1 = (float)i / ((float)numVertices - 1.0f);
      float r2 = (1.0f - Mathf.Abs(r1 * 2.0f - 1.0f) * 0.5f) + 0.5f;

      points_[i] = p1 + n1 * Mathf.Lerp(0.0f, d.magnitude, r1);
      points_[i] += n2 * h * r2;
    }

    renderer_.positionCount = numVertices;
    renderer_.SetPositions(points_);
  }

	void FixedUpdate()
  {
    if (pointA == null || pointB == null)
    {
      return;
    }

    Reconstruct();
	}

  void OnDrawGizmos()
  {
    if (pointA == null || pointB == null)
    {
      return;
    }

    Gizmos.color = Color.cyan;

    Vector3 p1 = pointA.position;
    Vector3 p2 = pointB.position;

    Gizmos.DrawLine(p1, p2);

    float s = 0.25f;

    Gizmos.DrawWireCube(p1, new Vector3(s, s, s));
    Gizmos.DrawWireCube(p2, new Vector3(s, s, s));
  }
}
