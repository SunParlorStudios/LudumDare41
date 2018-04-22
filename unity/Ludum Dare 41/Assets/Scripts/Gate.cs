using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateState
{
  kClosed,
  kOpened
}

public enum GateTriggerType
{
  kPressurePlate,
  kSwitch,
  kTriggerZone,
  kNoTrigger
}

public class Gate : MonoBehaviour
{
  public GateState initialState;
  public GateTriggerType triggerType;
  public Switch triggerSwitch;
  public PressurePlate triggerPlate;
  public GateState state
  { get { return state_; } }

  private GateState state_;
  private Animator animator_;
  private int numObjectsInTriggerZone_;

  private AudioSource audio_;

	void Start ()
  {
    animator_ = GetComponent<Animator>();
    numObjectsInTriggerZone_ = 0;

    audio_ = GetComponent<AudioSource>();

    state_ = initialState;
    animator_.SetInteger("State", (int)state_);

    if (triggerType == GateTriggerType.kPressurePlate && triggerPlate == null)
    {
      Debug.LogWarning("Trigger type is PressurePlate but no pressure plate was linked on Gate with name " + name + ". Trigger type will be set to no trigger.");

      triggerType = GateTriggerType.kNoTrigger;
    }

    if (triggerType == GateTriggerType.kSwitch && triggerSwitch == null)
    {
      Debug.LogWarning("Trigger type is Switch but no switch was linked on Gate with name " + name + ". Trigger type will be set to no trigger.");

      triggerType = GateTriggerType.kNoTrigger;
    }

    if (triggerType == GateTriggerType.kPressurePlate)
    {
      triggerPlate.PressurePlateEvent += PressurePlateListener;
    }

    if (triggerType == GateTriggerType.kSwitch)
    {
      triggerSwitch.SwitchEvent += SwitchListener;
    }
  }
	
	void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Alpha0))
    {
      animator_.SetInteger("State", (int)GateState.kClosed);
    }
    if (Input.GetKeyDown(KeyCode.Alpha9))
    {
      animator_.SetInteger("State", (int)GateState.kOpened);
    }
  }

  void TryOpen()
  {
    animator_.SetInteger("State", (int)GateState.kOpened);
    audio_.Play();
  }

  void TryClose(bool ignoreObjectsInTriggerZones = false)
  {
    if (!ignoreObjectsInTriggerZones && numObjectsInTriggerZone_ > 0)
    {
      return;
    }

    animator_.SetInteger("State", (int)GateState.kClosed);
    audio_.Play();
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    numObjectsInTriggerZone_++;

    if (triggerType == GateTriggerType.kTriggerZone)
    {
      TryOpen();
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    numObjectsInTriggerZone_--;

    if (triggerType == GateTriggerType.kTriggerZone)
    {
      TryClose();
    }
  }

  void PressurePlateListener(PressurePlateState oldState, PressurePlateState newState)
  {
    switch (newState)
    {
      case PressurePlateState.kDown:
        TryOpen();
        break;
      case PressurePlateState.kUp:
        TryClose(true);
        break;
    }
  }

  void SwitchListener(SwitchState oldState, SwitchState newState)
  {
    switch (newState)
    {
      case SwitchState.kLeft:
        TryClose(true);
        break;
      case SwitchState.kRight:
        TryOpen();
        break;
    }
  }
}
