using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlare : MonoBehaviour
{
  public float bias;
  public float flickerSpeed;

  [Range(0.0f, 1.0f)]
  public float opacityFactor;

  private Color startColor_;
  private Color toColor_;
  private Material material_;
  private float startOpacity_;

  private float timer_;

  private void Start()
  {
    material_ = GetComponent<MeshRenderer>().material;
    OverrideColor(material_.GetColor("_TintColor"));
  }

  public void OverrideColor(Color color)
  {
    startColor_ = color;
    toColor_.a = 0.0f;
    startOpacity_ = startColor_.a;
  }

  void Update()
  {
    transform.LookAt(Camera.main.transform);

    Vector3 d = Camera.main.transform.position - transform.position;
    float angle = Mathf.Atan2(d.z, d.x);
    
    angle += Mathf.PI * 0.5f;

    float ratio = Mathf.Clamp(Mathf.Abs(angle), 0.0f, 1.0f);

    ratio += Mathf.Lerp(0.0f, bias, ratio);
    ratio = Mathf.Clamp(ratio, 0.0f, 1.0f);

    float o1 = startOpacity_ * opacityFactor;
    float o2 = (1.0f - opacityFactor) * startOpacity_;

    timer_ += Time.deltaTime * flickerSpeed;
    startColor_.a = o1 + Mathf.Abs(Mathf.Cos(timer_ * 100.0f)) * Mathf.Abs(Mathf.Sin(timer_)) * o2;

    while (timer_ > Mathf.PI * 2.0f)
    {
      timer_ -= Mathf.PI * 2.0f;
    }

    material_.SetColor("_TintColor", Color.Lerp(startColor_, toColor_, ratio));
	}
}
