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
  public bool skipIntro = false;

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
    GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

    followCamera_ = camera.GetComponent<FollowCamera>();
    overviewCamera_ = camera.GetComponent<OverviewCamera>();
    
    state_ = GameState.LevelOverview;

    followCamera_.enabled = false;
    overviewCamera_.enabled = true;
    countdown.enabled = false;

    overviewCamera_.OverviewFinishedEvent += OverviewFinishedListener;
    countdown.CountdownFinishedEvent += CountdownFinishedListener;

    if (skipIntro)
    {
      state_ = GameState.Game;
      followCamera_.enabled = true;
      overviewCamera_.enabled = false;
      countdown.enabled = false;
    }
  }

  void Update()
  {
    switch (state_)
    {
      case GameState.LevelOverview:
      case GameState.Countdown:
        if (Input.GetKeyDown(KeyCode.Space))
        {
          state_ = GameState.Game;
          overviewCamera_.Skip();
          countdown.Skip();
          followCamera_.enabled = true;
        }
        break;
    }
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
