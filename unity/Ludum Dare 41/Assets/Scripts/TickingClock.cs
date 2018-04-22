using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TickingClock : MonoBehaviour
{
  public List<float> playbackIntervals;
  public List<float> thresholds;

  [HideInInspector]
  public float normalizedTimeLeft;
  [HideInInspector]
  public bool playing
  {
    get { return playing_; }
    set { playing_ = value; if (value == true) DoTick(); }
  }

  private bool playing_;

  private float timeLeft = 20.0f;
  private float lastTickTime_;

  private AudioSource source_;

	void Start ()
  {
    playing = false;
    source_ = GetComponent<AudioSource>();

    if (playbackIntervals.Count == 0 || thresholds.Count == 0 || playbackIntervals.Count != thresholds.Count)
    {
      playbackIntervals = new List<float>();
      playbackIntervals.Add(1.0f);
      playbackIntervals.Add(0.7f);
      playbackIntervals.Add(0.5f);
      playbackIntervals.Add(0.25f);
      playbackIntervals.Add(0.1f);

      thresholds = new List<float>();
      thresholds.Add(0.8f);
      thresholds.Add(0.4f);
      thresholds.Add(0.2f);
      thresholds.Add(0.1f);
      thresholds.Add(0.0f);
    }
  }
	
	void Update ()
  {
    if (playing)
    {
      for (int i = 0; i < thresholds.Count; i++)
      {
        if (normalizedTimeLeft >= thresholds[i])
        {
          if (Time.time - lastTickTime_ > playbackIntervals[i])
          {
            DoTick();
          }

          break;
        }
      }
    }
	}

  public void DoTick()
  {
    if (!source_.isPlaying)
    {
      source_.Play();
      lastTickTime_ = Time.time;
    }
  }
}
