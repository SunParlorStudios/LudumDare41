using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  public float lifetime;

  private float timer_;

	void Update()
  {
    timer_ += Time.deltaTime;

    float r = timer_ / lifetime;

    r = Mathf.Clamp(r, 0.0f, 1.0f);

    if (r >= 1.0f)
    {
      Destroy(gameObject);
    }

    Vector3 s = transform.localScale;
    s.y = 0.0f;

    transform.localScale = Vector3.Lerp(transform.localScale, s, r);
	}
}
