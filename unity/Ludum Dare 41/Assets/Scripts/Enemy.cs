using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public enum State
  {
    kHovering,
    kAttacking
  };

  public float aggroRange;
  public float wobbleSpeed;
  public float wobbleHeight;
  public float attackStepSize;

  private State state_;
  private Player target_;

  private Vector3 current_;
  private float wobbleTimer_;

  void Awake()
  {
    state_ = State.kHovering;
    target_ = null;
  }

  void Start()
  {
    current_ = transform.position;
    target_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

  private bool ShouldAttack()
  {
    Vector2 p1 = transform.position;
    Vector2 p2 = target_.transform.position;

    Vector2 d = p2 - p1;

    if (Physics2D.Raycast(p1, d.normalized, aggroRange) == false)
    {
      if (d.magnitude <= aggroRange)
      {
        return true;
      }
    }

    return false;
  }

  private void Hover()
  {
    wobbleTimer_ += wobbleSpeed * Time.deltaTime;

    float pi2 = Mathf.PI * 2.0f;

    while (wobbleTimer_ > pi2)
    {
      wobbleTimer_ -= pi2;
    }

    transform.position = current_ + Vector3.up * Mathf.Sin(wobbleTimer_) * wobbleHeight;

    state_ = ShouldAttack() == true ? State.kAttacking : State.kHovering;
  }

  private void Attack()
  {
    transform.position = Vector3.Lerp(transform.position, target_.transform.position, attackStepSize);
  }

	void Update()
  {
		switch (state_)
    {
      case State.kHovering:
        Hover();
        break;

      default:
        break;
    }
	}

  void FixedUpdate()
  {
    switch (state_)
    {
      case State.kAttacking:
        Attack();
        break;

      default:
        break;
    }
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, aggroRange);
  }
}
