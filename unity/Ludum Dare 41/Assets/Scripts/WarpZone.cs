using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{
  public WarpZone connectedTo;
  public GameObject particlePrefab;

  private bool warpedTo_;
  private Player.Direction warpDirection_;

  void Start()
  {
    warpedTo_ = false;
  }
  
  void OnTriggerExit2D(Collider2D other)
  {
    warpedTo_ = false;
  }

  void OnTriggerStay2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      Player.Direction current = other.gameObject.GetComponent<Player>().GetDirection();

      if (current != warpDirection_)
      {
        Warp(other.gameObject, current);
      }
    }
  }

  void CreateParticle(Vector3 p)
  {
    Instantiate(particlePrefab, p, Quaternion.identity, null);
  }

  void Warp(GameObject go, Player.Direction direction)
  {
    CreateParticle(go.transform.position);

    Vector3 offset = go.transform.position - transform.position;

    go.transform.position = connectedTo.transform.position + offset;
    connectedTo.warpedTo_ = true;
    connectedTo.warpDirection_ = direction;

    CreateParticle(go.transform.position);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (connectedTo == null || warpedTo_ == true)
    {
      return;
    }

    if (other.gameObject.tag == "Player")
    {
      Warp(other.gameObject, other.gameObject.GetComponent<Player>().GetDirection());
    }
  }
}
