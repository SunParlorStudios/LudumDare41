using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewCamera : MonoBehaviour
{
  public delegate void OverviewFinishedListener();

  public List<Transform> overviewPoints;
  public float pointToPointDuration = 1.0f;
  public float cooldownDuration = 0.5f;

  public OverviewFinishedListener OverviewFinishedEvent;

  private bool coolingDown_ = true;
  private Vector3 originalPosition_;
  private int currentPoint_ = 0;
  private float animationTime_ = 0.0f;

	void Start ()
  {
    originalPosition_ = transform.position;
    currentPoint_ = 0;
    animationTime_ = 0.0f;

    for (int i = 0; i < overviewPoints.Count; i++)
    {
      if (overviewPoints[i] == null)
      {
        Debug.LogWarning("An overview point is set to null. Overview state will be skipped.");

        enabled = false;

        if (OverviewFinishedEvent != null)
        {
          OverviewFinishedEvent.Invoke();
        }

        break;
      }
    }
	}
	
	void Update ()
  {
    animationTime_ += Time.deltaTime;

    if (coolingDown_)
    {
      if (animationTime_ >= cooldownDuration)
      {
        coolingDown_ = false;
        animationTime_ = 0.0f;
      }
    }
    else
    {
      if (animationTime_ >= pointToPointDuration)
      {
        coolingDown_ = true;
        animationTime_ = 0.0f;
        currentPoint_++;

        if (currentPoint_ > overviewPoints.Count)
        {
          if (OverviewFinishedEvent != null)
          {
            OverviewFinishedEvent.Invoke();
          }

          enabled = false;
          return;
        }
      }
      
      Vector3 from, to;

      if (currentPoint_ == 0)
      {
        from = originalPosition_;
        to = overviewPoints[currentPoint_].position;
      }
      else if (currentPoint_ >= overviewPoints.Count)
      {
        from = overviewPoints[currentPoint_ - 1].position;
        to = originalPosition_;
      }
      else
      {
        from = overviewPoints[currentPoint_ - 1].position;
        to = overviewPoints[currentPoint_].position;
      }

      float t = animationTime_ / pointToPointDuration;

      transform.position = Vector3.Lerp(from, to, Easings.EaseOutInCubic(t, 0, 1));
      transform.position = new Vector3(transform.position.x, transform.position.y, originalPosition_.z);
    }
	}
}
