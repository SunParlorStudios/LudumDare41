using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
  LevelOverview,
  Countdown,
  Game,
  PostGame
}

public class Game : MonoBehaviour
{
  public Countdown countdown;

  public GameState state
  {
    get
    {
      return state_;
    }
  }

  private GameState state_;
  private FollowCamera followCamera_;
  private OverviewCamera overviewCamera_;

  void Awake()
  {
    state_ = GameState.LevelOverview;

    GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

    followCamera_ = camera.GetComponent<FollowCamera>();
    overviewCamera_ = camera.GetComponent<OverviewCamera>();

    followCamera_.enabled = false;
    overviewCamera_.enabled = true;
    countdown.enabled = false;

    overviewCamera_.OverviewFinishedEvent += OverviewFinishedListener;
    countdown.CountdownFinishedEvent += CountdownFinishedListener;
  }

  void Update()
  {
    
  }

  void OverviewFinishedListener()
  {
    state_ = GameState.Countdown;

    overviewCamera_.enabled = false;
    followCamera_.enabled = true;

    countdown.enabled = true;
  }

  void CountdownFinishedListener()
  {
    state_ = GameState.Game;
  }
}
