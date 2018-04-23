using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TypeWriter))]
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
  public GameObject deathPrefab;
  public Reticle reticle;
  public bool isCarrying;
  public float pickupDistance = 2.0f;
  public BoxCollider2D pickupCollider;
  public Vector2 pickupThrowForce;

  private Direction direction_;
  private Rigidbody2D rigidBody_;
  private TypeWriter typeWriter_;

  private Enemy lockedOn_;

  private bool grounded_;

  private Game game_;

  private Vector3 startLocation_;
  private bool alive_;

  public bool alive
  {
    get { return alive_; }
  }

  public float wobbleSpeed;
  public Transform mainHinge;

  public float topHingeAngle;
  public float bottomHingeAngle;

  public float fallHingeAngle;
  public float landHingeAngle;

  public float velocityFactor;

  [Range(0.0f, 1.0f)]
  public float hingeSmoothing;

  private float wobbleTimer_;
  private bool prevGrounded_;
  private float baseHingePosition_;

  private FadeToBlack fadeToBlack_;
  private GameObject reminder_;

  private AudioSource[] audioSources_;

  void Awake()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    fadeToBlack_ = GameObject.FindGameObjectWithTag("FadeToBlack").GetComponent<FadeToBlack>();
    reminder_ = GameObject.FindGameObjectWithTag("Reminder");
    reminder_.SetActive(false);

    direction_ = Direction.kRight;
    rigidBody_ = GetComponent<Rigidbody2D>();
    typeWriter_ = GetComponent<TypeWriter>();
    lockedOn_ = null;
    alive_ = true;
    isCarrying = false;
    wobbleTimer_ = 0.0f;
    prevGrounded_ = false;
    baseHingePosition_ = 0.0f;
    audioSources_ = GetComponents<AudioSource>();
  }

  void Start()
  {
    typeWriter_.RegisterWord("jump", OnWord);
    typeWriter_.RegisterWord("turn", OnWord);
    typeWriter_.RegisterWord("shoot", OnWord);
    typeWriter_.RegisterWord("reset", OnWord);
    typeWriter_.RegisterWord("kys", OnWord);
    typeWriter_.RegisterWord("next", OnWord);
    typeWriter_.RegisterWord("menu", OnWord);
    typeWriter_.RegisterWord("tutorial1", OnWord);
    typeWriter_.RegisterWord("tutorial2", OnWord);
    typeWriter_.RegisterWord("tutorial3", OnWord);
    typeWriter_.RegisterWord("tutorial4", OnWord);
    typeWriter_.RegisterWord("tutorial5", OnWord);
    typeWriter_.RegisterWord("tutorial6", OnWord);
    typeWriter_.RegisterWord("level1", OnWord);
    typeWriter_.RegisterWord("level2", OnWord);
    typeWriter_.RegisterWord("level3", OnWord);
    typeWriter_.RegisterWord("level4", OnWord);
    typeWriter_.RegisterWord("level5", OnWord);

    startLocation_ = transform.position;

    audioSources_[1].Play();
  }

  void Respawn()
  {
    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
  }

  public Direction GetDirection()
  {
    return direction_;
  }

  void OnWord(string word)
  {
    switch (word)
    {
      case "jump":
        Jump();
        break;

      case "turn":
        SwitchDirection();
        break;

      case "shoot":
        Shoot();
        break;

      case "reset":
        Respawn();
        break;

      case "kys":
        Kill();
        break;

      case "menu":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        break;

      case "next":
        UnityEngine.SceneManagement.SceneManager.LoadScene(game_.nextSceneName);
        break;


      case "tutorial1":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial01");
        break;
      case "tutorial2":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial02");
        break;
      case "tutorial3":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial03");
        break;
      case "tutorial4":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial04");
        break;
      case "tutorial5":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial05");
        break;
      case "tutorial6":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial06");
        break;
      case "level1":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
        break;
      case "level2":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level02");
        break;
      case "level3":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level03");
        break;
      case "level4":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level04");
        break;
      case "level5":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level05");
        break;

      default:
        break;
    }
  }

  private void SwitchDirection()
  {
    direction_ = direction_ == Direction.kLeft ? Direction.kRight : Direction.kLeft;

    Vector3 scale = transform.localScale;
    scale.x = direction_ == Direction.kLeft ? -1.0f : 1.0f;

    transform.localScale = scale;
  }

  private void LockOn()
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

  private void Shoot()
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
      lockedOn_.Kill();
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

    if (grounded == true && prevGrounded_ == false)
    {
      baseHingePosition_ = landHingeAngle;
      audioSources_[3].Play();
    }

    SetParticlesEnabled(grounded == true);

    return grounded;
  }

  private void SetParticlesEnabled(bool enabled)
  {
    ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

    foreach (ParticleSystem p in systems)
    {
      ParticleSystem.EmissionModule m = p.emission;
      m.enabled = enabled;
    }
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
    if (game_.state == GameState.Game)
    {
      if (rigidBody_ == null)
      {
        return;
      }

      AddDirectionalForce();
      ClampVelocity();
    }

    UpdateHinge();

    prevGrounded_ = grounded_;
  }

  void UpdateHinge()
  {
    wobbleTimer_ += Time.fixedDeltaTime * wobbleSpeed;

    while (wobbleTimer_ > Mathf.PI * 2.0f)
    {
      wobbleTimer_ -= Mathf.PI * 2.0f;
    }

    float r = (Mathf.Sin(wobbleTimer_) + 1.0f) * 0.5f;
    float a = Mathf.Lerp(bottomHingeAngle, topHingeAngle, r);

    if (rigidBody_.velocity.y < 0.0f)
    {
      float v = Mathf.Clamp(Mathf.Abs(rigidBody_.velocity.y) / velocityFactor, 0.0f, 1.0f);
      a = Mathf.Lerp(a, fallHingeAngle, v);
    }

    baseHingePosition_ = Mathf.Lerp(baseHingePosition_, 0.0f, hingeSmoothing);
    mainHinge.localRotation = Quaternion.Euler(0.0f, 0.0f, baseHingePosition_ + a);
  }

  void Update()
  {
    if (game_.state == GameState.Game)
    {
      grounded_ = CheckGrounded();
      LockOn();
    }
  }

  void SetVisible(bool enabled)
  {
    foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
    {
      mr.enabled = enabled;
    }
  }

  public void Kill()
  {
    if (alive_ == false)
    {
      return;
    }

    if (game_.state == GameState.Game)
    {
      alive_ = false;
      rigidBody_.velocity = Vector3.zero;
      rigidBody_.simulated = false;

      enabled = false;
      SetVisible(false);
      Instantiate(deathPrefab, transform.position, Quaternion.identity, null);
      SetParticlesEnabled(false);
      audioSources_[1].Stop();

      fadeToBlack_.Fade(4.0f);
      reminder_.SetActive(true);
    }
  }
}
