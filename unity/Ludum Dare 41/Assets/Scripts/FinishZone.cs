using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
  private Game game_;

  public GameObject particleEffect;
  public Transform particleSpawnPoint;

  void Start()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
  }

  void Update()
  {

  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      game_.reachedFinish = true;

      if (particleEffect != null && game_.HasMetWinConditions())
      {
        Instantiate(particleEffect, particleSpawnPoint);
      }
    }
  }

  void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      game_.reachedFinish = false;
    }
  }
}
