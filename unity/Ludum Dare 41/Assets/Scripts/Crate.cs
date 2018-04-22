using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Crate : MonoBehaviour
{
  private Player player_;

  private bool isCarried_;
  private Rigidbody2D rigidbody_;
  private BoxCollider2D collider_;

  void Start()
  {
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

    rigidbody_ = GetComponent<Rigidbody2D>();
    collider_ = GetComponent<BoxCollider2D>();

    if (playerObject != null)
    {
      player_ = playerObject.GetComponent<Player>();

      TypeWriter typeWriter = playerObject.GetComponent<TypeWriter>();
      typeWriter.RegisterWord("use", WordListener);
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
    if (!isCarried_)
    {
      TryPickup();
    }
    else
    {
      Drop();
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
}
