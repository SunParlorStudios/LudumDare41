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

  public PressurePlateState state
  {
    get { return state_; }
  }

  private PressurePlateState state_;
  private Animator animator_;

  private int objectsOnTop_ = 0;

  void Start()
  {
    animator_ = GetComponent<Animator>();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha0))
    {
      animator_.SetInteger("State", 0);
    }

    if (Input.GetKeyDown(KeyCode.Alpha9))
    {
      animator_.SetInteger("State", 1);
    }
  }

  public void TryPress()
  {
    if (state_ == PressurePlateState.kUp)
    {
      state_ = PressurePlateState.kDown;
      animator_.SetInteger("State", (int)PressurePlateState.kDown);

      if (PressurePlateEvent != null)
      {
        PressurePlateEvent.Invoke(PressurePlateState.kUp, PressurePlateState.kDown);
      }
    }
  }

  public void TryLift()
  {
    if (objectsOnTop_ == 0)
    {
      state_ = PressurePlateState.kUp;
      animator_.SetInteger("State", (int)PressurePlateState.kUp);

      if (PressurePlateEvent != null)
      {
        PressurePlateEvent.Invoke(PressurePlateState.kUp, PressurePlateState.kDown);
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
