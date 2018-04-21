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
  private TypeWriter typeWriter_;

  private bool grounded_;

	void Awake()
  {
		direction_ = Direction.kRight;
    rigidBody_ = GetComponent<Rigidbody2D>();
    typeWriter_ = GetComponent<TypeWriter>();
	}

  void Start()
  {
    typeWriter_.RegisterWord("jump", Jump);
    typeWriter_.RegisterWord("turn", SwitchDirection);
  }

  private void SwitchDirection(string word)
  {
    direction_ = direction_ == Direction.kLeft ? Direction.kRight : Direction.kLeft;

    Vector3 scale = transform.localScale;
    scale.x = direction_ == Direction.kLeft ? -1.0f : 1.0f;

    transform.localScale = scale;
  }

  private bool CheckGrounded()
  {
    bool grounded = false;
    if (Physics2D.Raycast(transform.position, Vector2.down, jumpCheckRay) == true)
    {
      grounded = true;
    }

    Debug.DrawLine(transform.position, transform.position + Vector3.down * jumpCheckRay,
      grounded == true ? Color.green : Color.red);

    return grounded;
  }

  private void Jump(string word)
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
