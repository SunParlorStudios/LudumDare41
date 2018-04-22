using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
  public delegate void ButtonListener();
  public ButtonListener ButtonPressedEvent;
  public ButtonListener ButtonReleasedEvent;

  public float duration = 5;
  public Color idleColor;
  public Color pressedColor;
  public MeshRenderer buttonRenderer;
  public GameObject pressParticle;
  public bool isPressed
  { get { return countdown_ != 0.0f; } }

  private float countdown_ = 0;
  private Player player_;
  private TypeWriter typeWriter_;
  private bool playerIsNear_ = false;
  private bool isAnimating_;

  void Start()
  {
    isAnimating_ = false;
    countdown_ = 0;
    player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    typeWriter_ = player_.GetComponent<TypeWriter>();
    playerIsNear_ = false;

    typeWriter_.RegisterWord("use", WordListener);
  }

  void WordListener(string word)
  {
    TryPush();
  }

  void Update()
  {
    if (isAnimating_)
    {
      countdown_ = countdown_ - Time.deltaTime;

      if (countdown_ <= 0.0f)
      {
        isAnimating_ = false;

        if (ButtonReleasedEvent != null)
        {
          ButtonReleasedEvent.Invoke();
        }
      }

      countdown_ = Mathf.Clamp(countdown_, 0, duration);

      buttonRenderer.material.color = Color.Lerp(idleColor, pressedColor, countdown_ / duration);
    }
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
      playerIsNear_ = true;
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.tag == "Player")
      playerIsNear_ = false;
  }

  void TryPush()
  {
    if (playerIsNear_)
    {
      if (ButtonPressedEvent != null)
      {
        ButtonPressedEvent.Invoke();
      }

      countdown_ = duration;
      isAnimating_ = true;

      GameObject particle = Instantiate(pressParticle);
      particle.transform.position = transform.position + Vector3.up;
    }
  }
}
