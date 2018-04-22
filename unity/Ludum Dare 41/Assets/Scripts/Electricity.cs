using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Electricity : MonoBehaviour
{
  public enum TriggerType
  {
    kAlwaysOn,
    kSwitch,
    kPressurePlate
  }

  public TriggerType triggeredBy;

  public Transform pointA;
  public Transform pointB;

  private Light lightA_;
  private Light lightB_;

  private float startIntensityA_;
  private float startIntensityB_;
  
  [Range(0.01f, 1.0f)]
  public float stepRatio;

  public float width;

  public PressurePlate triggerPlate;
  public Switch triggerSwitch;

  public KillZone killZone;

  private LineRenderer renderer_;
  private Vector3[] points_;


	void Start()
  {
    renderer_ = GetComponent<LineRenderer>();

    if (triggerPlate != null && triggeredBy == TriggerType.kPressurePlate)
    {
      triggerPlate.PressurePlateEvent += PressurePlateListener;
    }
    else if (triggerSwitch != null && triggeredBy == TriggerType.kSwitch)
    {
      triggerSwitch.SwitchEvent += SwitchListener;
    }

    lightA_ = pointA.GetComponent<Light>();
    lightB_ = pointB.GetComponent<Light>();
    startIntensityA_ = lightA_.intensity;
    startIntensityB_ = lightB_.intensity;
  }

  void RepositionKillZone(Vector3 p1, Vector3 p2)
  {
    if (killZone == null)
    {
      return;
    }

    Vector3 d = p2 - p1;

    Vector3 n = d.normalized;

    Vector3 halfWay = p1 + n * d.magnitude * 0.5f;
    killZone.transform.position = halfWay;

    float a = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;

    killZone.transform.rotation = Quaternion.Euler(0.0f, 0.0f, a);

    BoxCollider2D col = killZone.GetComponent<BoxCollider2D>();
    Vector2 s = col.size;
    s.x = d.magnitude;
    col.size = s;
  }

  void Reconstruct()
  {
    Vector3 p1 = pointA.position;
    Vector3 p2 = pointB.position;

    Vector3 d = p2 - p1;

    Vector3 n1 = d.normalized;
    Vector3 n2 = new Vector3(d.y, -d.x, 0.0f);
    
    RepositionKillZone(p1, p2);

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

  protected void SetEnabled(bool e)
  {
    killZone.gameObject.SetActive(e);
    renderer_.enabled = e;
    enabled = e;
  }

  public void PressurePlateListener(PressurePlateState oldState, PressurePlateState newState)
  {
    SetEnabled(newState != PressurePlateState.kDown);
  }

  public void SwitchListener(SwitchState oldState, SwitchState newState)
  {
    SetEnabled(newState == SwitchState.kLeft);
  }

	void FixedUpdate()
  {
    if (pointA == null || pointB == null)
    {
      return;
    }

    Reconstruct();
    Flicker();
	}

  void Flicker()
  {
    float a = Random.Range(startIntensityA_ * 0.5f, startIntensityA_);
    float b = Random.Range(startIntensityB_ * 0.5f, startIntensityB_);

    lightA_.intensity = a;
    lightB_.intensity = b;
  }

  void OnDrawGizmos()
  {
    if (pointA == null || pointB == null || killZone == null)
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

    RepositionKillZone(p1, p2);
  }
}
