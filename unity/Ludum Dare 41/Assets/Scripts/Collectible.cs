using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
  private Game game_;
  private Animator animator_;
  private AudioSource audio_;
  private MeshRenderer[] renderer_;

	void Start ()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    audio_ = GetComponent<AudioSource>();
    renderer_ = GetComponentsInChildren<MeshRenderer>();
    animator_ = GetComponent<Animator>();

    game_.numCollectiblesAlive++;
	}
	
	void Update ()
  {
    
	}

  void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.tag == "Player")
    {
      audio_.Play();
      animator_.SetTrigger("Pickup");
    }
  }

  void PickupAnimationComplete()
  {
    game_.numCollectiblesAlive--;
    Destroy(gameObject);
  }
}
