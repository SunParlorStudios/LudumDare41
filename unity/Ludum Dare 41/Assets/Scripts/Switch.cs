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

  void Start()
  {
    animator_ = GetComponent<Animator>();
    nextState_ = SwitchState.kRight;
    playerInRange_ = false;

    SwitchTo(initialState, false);

    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    if (playerObject != null)
    {
      TypeWriter typeWriter = playerObject.GetComponent<TypeWriter>();
      typeWriter.RegisterWord("use", WordListener);
    }
    else
    {
      Debug.LogWarning("Couldn't find player object in the scene and therefore not the TypeWriter. Switches won't subscribe to any words in the TypeWriter now.");
    }
  }

  void Update()
  {
    
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
