using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZone : MonoBehaviour
{
  public Transform doorPoint;

  private Game game_;
  private Player player_;
  private Animator animator_;
  private bool playerGettingAnimated_ = false;
  
  void Start()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    animator_ = GetComponent<Animator>();
  }

  void Update()
  {
    if (playerGettingAnimated_)
    {
      player_.transform.position = Vector3.Lerp(player_.transform.position, doorPoint.position, 0.01f);
    }
  }

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      game_.reachedFinish = true;

      if (game_.HasMetWinConditions())
      {
        player_.enabled = false;
        player_.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerGettingAnimated_ = true;
        animator_.SetTrigger("Finished");
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
