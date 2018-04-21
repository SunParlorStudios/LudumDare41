using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
  PreGame,
  Game,
  PostGame
}

public class Game : MonoBehaviour
{
  public GameState state
  {
    get
    {
      return state_;
    }
  }

  private GameState state_;

  void Start()
  {
    state_ = GameState.PreGame;
  }

  void Update()
  {

  }
}
