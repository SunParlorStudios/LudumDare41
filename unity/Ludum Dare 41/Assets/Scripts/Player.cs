using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public enum Direction
  {
    kLeft,
    kRight
  };

  public float acceleration;
  public float maxSpeed;
  public float jumpForce;
  public float jumpCheckRay;

  private Direction direction_;
  private Rigidbody2D rigidBody_;

  private bool grounded_;

	void Awake()
  {
		direction_ = Direction.kRight;
    rigidBody_ = GetComponent<Rigidbody2D>();
	}

  private void SwitchDirection()
  {
    direction_ = direction_ == Direction.kLeft ? Direction.kRight : Direction.kLeft;
  }

  private bool CheckGrounded()
  {
    bool grounded = false;
    if (Physics2D.Raycast(transform.position, Vector2.down, jumpCheckRay) == true)
    {
      if (Vector3.Dot(rigidBody_.velocity, Vector3.up) <= 0.0f)
      {
        grounded =  true;
      }
    }

    Debug.DrawLine(transform.position, transform.position + Vector3.down * jumpCheckRay,
      grounded == true ? Color.green : Color.red);

    return grounded;
  }

  private void Jump()
  {
    if (grounded_ == false)
    {
      return;
    }

    grounded_ = false;
    rigidBody_.AddForce(Vector3.up * jumpForce);
  }

  private void AddDirectionalForce()
  {
    Vector3 force = Vector3.zero;

    switch (direction_)
    {
      case Direction.kLeft:
        force = Vector3.left;
      break;

      case Direction.kRight:
        force = Vector3.right;
      break;
    }

    force *= acceleration * Time.fixedDeltaTime;

    rigidBody_.AddForce(force);
  }

  private void ClampVelocity()
  {
    Vector3 v = rigidBody_.velocity;

    float spd = v.x;
    float abs = Mathf.Abs(spd);

    float sign = spd == 0.0f ? 0.0f : abs / spd;

    spd = Mathf.Clamp(abs, 0.0f, maxSpeed);

    v.x = spd * sign;

    rigidBody_.velocity = v;
  }
  
  void FixedUpdate()
  {
    if (rigidBody_ == null)
    {
      return;
    }

    AddDirectionalForce();
    ClampVelocity();
  }

  void Update()
  {
    grounded_ = CheckGrounded();
  }
}
