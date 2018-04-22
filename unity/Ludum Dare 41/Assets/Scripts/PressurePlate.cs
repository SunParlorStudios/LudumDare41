using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PressurePlateState
{
  kUp,
  kDown
}

public class PressurePlate : MonoBehaviour
{
  public delegate void PressurePlateListener(PressurePlateState oldState, PressurePlateState newState);
  public PressurePlateListener PressurePlateEvent;
  public float timePressedAfterReleased = -1;

  public AudioSource pressAudio;
  public AudioSource releaseAudio;

  public PressurePlateState state
  {
    get { return state_; }
  }

  private PressurePlateState state_;
  private Animator animator_;

  private int objectsOnTop_ = 0;

  private TickingClock clock_;
  private float timeElapsed_ = 0;

  void Start()
  {
    animator_ = GetComponent<Animator>();
    clock_ = GetComponentInChildren<TickingClock>();
  }

  void Update()
  {
    if (objectsOnTop_ == 0 && state_ == PressurePlateState.kDown)
    {
      clock_.playing = true;
      timeElapsed_ += Time.deltaTime;

      if (timePressedAfterReleased != -1)
      {
        clock_.normalizedTimeLeft = (timePressedAfterReleased - timeElapsed_) / timePressedAfterReleased;

        if (clock_.normalizedTimeLeft <= 0)
        {
          clock_.playing = false;
          TryLift();
        }
      }
    }
    else
    {
      timeElapsed_ = 0;
    }
  }

  public void TryPress()
  {
    if (state_ == PressurePlateState.kUp)
    {
      state_ = PressurePlateState.kDown;
      animator_.SetInteger("State", (int)PressurePlateState.kDown);

      pressAudio.Play();

      if (PressurePlateEvent != null)
      {
        PressurePlateEvent.Invoke(PressurePlateState.kUp, PressurePlateState.kDown);
      }
    }
  }

  public void TryLift()
  {
    if (timePressedAfterReleased == -1 || timeElapsed_ > timePressedAfterReleased)
    {
      if (objectsOnTop_ == 0)
      {
        state_ = PressurePlateState.kUp;
        animator_.SetInteger("State", (int)PressurePlateState.kUp);

        releaseAudio.Play();

        if (PressurePlateEvent != null)
        {
          PressurePlateEvent.Invoke(PressurePlateState.kDown, PressurePlateState.kUp);
        }
      }
    }
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    objectsOnTop_ = Mathf.Clamp(objectsOnTop_ + 1, 0, 100);
    TryPress();
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    objectsOnTop_ = Mathf.Clamp(objectsOnTop_ - 1, 0, 100);
    TryLift();
  }
}
