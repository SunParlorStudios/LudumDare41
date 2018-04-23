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

public enum WinCondition
{
  kReachFinish,
  kTimeLimit,
  kKillAllEnemies,
  kCollectAllEnemies
}

public class Game : MonoBehaviour
{
  public string nextSceneName = "Menu";

  public Countdown countdown;
  public bool skipIntro = false;

  public bool mustReachFinish = true;
  public bool mustCompleteInTime = false;
  public bool mustKillAll = false;
  public bool mustCollectAll = false;

  public float timeLimit = -1;

  [HideInInspector]
  public int numEnemiesAlive = 0;
  [HideInInspector]
  public int numCollectiblesAlive = 0;
  [HideInInspector]
  public bool reachedFinish = false;
  [HideInInspector]
  public float gameTime = 0.0f;

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
  private Player player_;
  private TypeWriter typeWriter_;
  private FadeToBlack fadeToBlack_;

  void Awake()
  {
    GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");

    fadeToBlack_ = GameObject.FindGameObjectWithTag("FadeToBlack").GetComponent<FadeToBlack>();

    player_ = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    typeWriter_ = player_.GetComponent<TypeWriter>();

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
      typeWriter_.allowInput = true;
    }
  }

  void Start()
  {
    fadeToBlack_.Unfade(2.5f);
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
          typeWriter_.allowInput = true;
          gameTime = 0.0f;
        }
        break;
      case GameState.Game:
        gameTime += Time.deltaTime;

        if (HasFailedLevel())
        {
          // failed
        }
        else
        {
          if (HasMetWinConditions())
          {
            typeWriter_.allowInput = false;
            state_ = GameState.PostGame;
          }
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
    gameTime = 0.0f;
  }

  void CountdownFinishedListener()
  {
    state_ = GameState.Game;
    typeWriter_.allowInput = true;
    gameTime = 0.0f;
  }

  public bool HasMetWinConditions()
  {
    bool winConditionsMet = true;

    if (mustCollectAll && numCollectiblesAlive > 0)
    {
      winConditionsMet = false;
    }

    if (mustKillAll && numEnemiesAlive > 0)
    {
      winConditionsMet = false;
    }

    if (mustReachFinish && !reachedFinish)
    {
      winConditionsMet = false;
    }

    return winConditionsMet;
  }

  public bool HasFailedLevel()
  {
    return mustCompleteInTime && gameTime > timeLimit;
  }
}
