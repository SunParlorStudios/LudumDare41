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
  public Vector3 offset;
  public GameObject explosionPrefab;

  private State state_;
  private Player target_;

  private Vector3 current_;
  private float wobbleTimer_;

  private Game game_;

  void Awake()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
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
    Vector3 p2 = target_.transform.position + offset;
    transform.position = Vector3.Lerp(transform.position, p2, attackStepSize);

    Vector3 d = p2 - transform.position;
    float angle = Mathf.Atan2(d.y, d.x);

    float sy = 1.0f;
    if (angle < -Mathf.PI * 0.5f || angle > Mathf.PI * 0.5f)
    {
      sy = -1.0f;
    }

    Vector3 s = transform.localScale;
    s.y = sy;
    transform.localScale = s;

    transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
  }

  public void Kill()
  {
    Destroy(gameObject);

    if (explosionPrefab != null)
    {
      Instantiate(explosionPrefab, transform.position, Quaternion.identity, null);
    }
  }

  void Update()
  {
    if (game_.state == GameState.Game)
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
  }

  void FixedUpdate()
  {
    if (game_.state == GameState.Game)
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
  }

  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, aggroRange);
  }
}
