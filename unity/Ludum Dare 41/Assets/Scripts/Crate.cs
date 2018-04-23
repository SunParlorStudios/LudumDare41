using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Crate : MonoBehaviour
{
  public List<AudioClip> clips;

  private AudioSource audio_;

  private Player player_;

  private bool isCarried_;
  private Rigidbody2D rigidbody_;
  private BoxCollider2D collider_;

  void Start()
  {
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    rigidbody_ = GetComponent<Rigidbody2D>();
    collider_ = GetComponent<BoxCollider2D>();
    audio_ = GetComponent<AudioSource>();

    if (playerObject != null)
    {
      player_ = playerObject.GetComponent<Player>();

      TypeWriter typeWriter = playerObject.GetComponent<TypeWriter>();
      typeWriter.RegisterWord("use", WordListener);
      typeWriter.RegisterWord("throw", WordListener);
    }
    else
    {
      Debug.LogWarning("Couldn't find player object in the scene. Crates won't function now.");
    }
  }

  void FixedUpdate()
  {
    if (isCarried_)
    {
      transform.position = player_.transform.position + (player_.transform.localScale.x * Vector3.right * 1.30f) + (Vector3.up * 0.35f);
    }
  }

  void WordListener(string word)
  {
    switch (word)
    {
      case "use":
        if (!isCarried_)
        {
          TryPickup();
        }
        else
        {
          Drop();
        }
        break;
      case "throw":
        if (isCarried_)
        {
          TryThrow();
        }
        break;
    }
  }

  void TryPickup()
  {
    if (!player_.isCarrying)
    {
      float dist = Vector3.Distance(player_.transform.position, transform.position);

      if (dist <= player_.pickupDistance)
      {
        player_.isCarrying = true;
        isCarried_ = true;
        rigidbody_.isKinematic = true;
        player_.pickupCollider.enabled = true;
        collider_.enabled = false;
        transform.rotation = Quaternion.identity;
      }
    }
  }

  void Drop()
  {
    player_.isCarrying = false;
    isCarried_ = false;
    rigidbody_.isKinematic = false;
    player_.pickupCollider.enabled = false;
    collider_.enabled = true;
  }

  void TryThrow()
  {
    Drop();

    int direction = 0;

    if (player_.transform.position.x < transform.position.x)
    {
      direction = 1;
    }
    else
    {
      direction = -1;
    }

    Vector3 force = new Vector3(player_.pickupThrowForce.x * direction, player_.pickupThrowForce.y);

    rigidbody_.AddForceAtPosition(force, transform.position + (Vector3.left * direction * 0.2f), ForceMode2D.Impulse);
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    //audio_.Play();
  }
}
