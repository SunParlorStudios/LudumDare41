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
  public float lockRange;
  public GameObject laserPrefab;
  public Reticle reticle;

  private Direction direction_;
  private Rigidbody2D rigidBody_;
  private TypeWriter typeWriter_;

  private Enemy lockedOn_;

  private bool grounded_;

	void Awake()
  {
		direction_ = Direction.kRight;
    rigidBody_ = GetComponent<Rigidbody2D>();
    typeWriter_ = GetComponent<TypeWriter>();
    lockedOn_ = null;
  }

  void Start()
  {
    typeWriter_.RegisterWord("jump", Jump);
    typeWriter_.RegisterWord("turn", SwitchDirection);
    typeWriter_.RegisterWord("lock", LockOn);
    typeWriter_.RegisterWord("shoot", Shoot);
  }

  private void SwitchDirection(string word)
  {
    direction_ = direction_ == Direction.kLeft ? Direction.kRight : Direction.kLeft;

    Vector3 scale = transform.localScale;
    scale.x = direction_ == Direction.kLeft ? -1.0f : 1.0f;

    transform.localScale = scale;
  }

  private void LockOn(string word)
  {
    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    float dist = 0.0f;
    GameObject nearest = null;

    foreach (GameObject go in enemies)
    {
      Vector3 d = go.transform.position - transform.position;
      float distance = d.magnitude;

      if (distance <= lockRange && (distance < dist || nearest == null))
      {
        nearest = go;
        dist = distance;
      }
    }

    if (nearest != null)
    {
      lockedOn_ = nearest.GetComponent<Enemy>();
      reticle.SetTarget(lockedOn_.transform);

      return;
    }

    reticle.SetTarget(null);
  }

  private void Shoot(string word)
  {
    if (lockedOn_ == null)
    {
      return;
    }

    Vector3 d = lockedOn_.transform.position - transform.position;

    RaycastHit2D hit = Physics2D.Raycast(transform.position, d.normalized, d.magnitude);
    
    if (hit == true)
    {
      Vector2 p1 = transform.position;
      d = hit.point - p1;
    }
    else
    {
      Destroy(lockedOn_.gameObject);
      lockedOn_ = null;
      reticle.SetTarget(null);
    }

    float distance = d.magnitude;

    float angle = Mathf.Atan2(d.y, d.x);

    Vector3 p = transform.position + d.normalized * distance * 0.5f;

    GameObject go = Instantiate(laserPrefab, p, Quaternion.identity, null);
    Vector3 s = go.transform.localScale;

    s.x = distance;

    go.transform.localScale = s;
    go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
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
