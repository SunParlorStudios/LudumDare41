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
  public PressurePlateState state
  {
    get { return state_; }
  }

  private PressurePlateState state_;
  private Animator animator_;

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
}
