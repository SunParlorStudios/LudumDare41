using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwitchState
{
  kLeft,
  kRight,
  kUp
}
public class Switch : MonoBehaviour
{
  public delegate void SwitchListener(SwitchState oldState, SwitchState newState);
  public SwitchListener SwitchEvent;

  public SwitchState initialState;
  public bool hasTristate = false;

  public float maxTimeInRightState = -1;
  public float maxTimeInLeftState = -1;
  public float maxTimeInUpState = -1;

  public SwitchState state
  {
    get
    {
      return state_;
    }
  }

  private Animator animator_;
  private SwitchState state_;
  private SwitchState nextState_;
  private bool playerInRange_;
  private TickingClock clock_;
  private float timeElapsed_ = 0;

  void Start()
  {
    animator_ = GetComponent<Animator>();
    nextState_ = SwitchState.kRight;
    playerInRange_ = false;

    SwitchTo(initialState, false);

    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    clock_ = GetComponentInChildren<TickingClock>();

    if (playerObject != null)
    {
      TypeWriter typeWriter = playerObject.GetComponent<TypeWriter>();
      typeWriter.RegisterWord("use", WordListener);
    }
    else
    {
      Debug.LogWarning("Couldn't find player object in the scene and therefore not the TypeWriter. Switches won't subscribe to any words in the TypeWriter now.");
    }

    timeElapsed_ = 0;
  }

  void Update()
  {
    timeElapsed_ += Time.deltaTime;

    float maxTime = -1;

    switch (state_)
    {
      case SwitchState.kLeft:
        maxTime = maxTimeInLeftState;
        break;
      case SwitchState.kRight:
        maxTime = maxTimeInRightState;
        break;
      case SwitchState.kUp:
        maxTime = maxTimeInUpState;
        break;
    }

    if (maxTime != -1)
    {
      clock_.normalizedTimeLeft = (maxTime - timeElapsed_) / maxTime;

      if (clock_.normalizedTimeLeft <= 0)
      {
        clock_.playing = false;
        Toggle();
      }
    }
  }
  
  public void WordListener(string word)
  {
    if (playerInRange_)
    {
      Toggle();
    }
  }

  public void SwitchTo(SwitchState newState, bool notifyListeners = true)
  {
    if (SwitchEvent != null && notifyListeners)
    {
      SwitchEvent.Invoke(state_, newState);
    }

    state_ = newState;
    animator_.SetInteger("State", (int)newState);

    switch(newState)
    {
      case SwitchState.kLeft:
        if (maxTimeInLeftState != -1)
        {
          clock_.playing = true;
        }
        break;
      case SwitchState.kRight:
        if (maxTimeInRightState != -1)
        {
          clock_.playing = true;
        }
        break;
      case SwitchState.kUp:
        if (maxTimeInUpState != -1)
        {
          clock_.playing = true;
        }
        break;
    }

    timeElapsed_ = 0;
  }

  public void Toggle(bool notifyListeners = true)
  {
    int newState = (int)SwitchState.kUp;

    if (!hasTristate)
    {
      newState = ((int)state_ + 1) % 2;
    }
    else
    {
      if (state_ == SwitchState.kUp)
      {
        newState = (int)nextState_;
      }
      else if (state_ == SwitchState.kLeft)
      {
        newState = (int)SwitchState.kUp;
        nextState_ = SwitchState.kRight;
      }
      else if (state_ == SwitchState.kRight)
      {
        newState = (int)SwitchState.kUp;
        nextState_ = SwitchState.kLeft;
      }
    }

    SwitchTo((SwitchState)newState, notifyListeners);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      playerInRange_ = true;
    }
  }

  private void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      playerInRange_ = false;
    }
  }
}
