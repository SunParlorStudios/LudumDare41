using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CantFinishReasonText : MonoBehaviour
{
  public string intro;
  public string collectiblesMissingText;
  public string enemiesAliveText;
  public string reachFinishText;

  private Game game_;
  private Text text_;

  private Color from_ = new Color(0.0f, 0.0f, 0.0f, 1.0f);
  private Color to_ = new Color(0.0f, 0.0f, 0.0f, 0.0f);

  private float animationDuration_ = 0.0f;
  private float elapsedTime_ = 0.0f;
  private bool animating_ = false;

  private bool textLocked_ = false;

  void Start ()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
    text_ = GetComponent<Text>();
	}
	
	void Update ()
  {
    if (!textLocked_)
    {
      if (!game_.HasMetWinConditions())
      {
        string completeText = intro + "\n\n";

        if (game_.mustKillAll && game_.numEnemiesAlive > 0)
        {
          completeText += enemiesAliveText + '\n';
        }

        if (game_.mustCollectAll && game_.numCollectiblesAlive > 0)
        {
          completeText += collectiblesMissingText + '\n';
        }

        if (game_.mustReachFinish && !game_.reachedFinish)
        {
          completeText += reachFinishText + '\n';
        }

        text_.text = completeText;
      }
      else
      {
        text_.text = "You've completed the level!\n\nType 'next' to go to the Next level\nType 'menu' to go to the menu\nType 'levelX' to go to a specific level";
      }
    }

    if (animating_)
    {
      elapsedTime_ += Time.deltaTime;
      float t = elapsedTime_ / animationDuration_;

      text_.color = Color.Lerp(from_, to_, t);

      if (t >= 1.0f)
      {
        animating_ = false;

        if (to_.a == 0.0f)
        {
          textLocked_ = false;
        }
      }
    }
  }

  public void Fade(float duration)
  {
    from_ = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    to_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    elapsedTime_ = 0.0f;
    animationDuration_ = duration;
    animating_ = true;

    textLocked_ = true;

    if (!game_.HasMetWinConditions())
    {
      string completeText = intro + "\n\n";

      if (game_.mustKillAll && game_.numEnemiesAlive > 0)
      {
        completeText += enemiesAliveText + '\n';
      }

      if (game_.mustCollectAll && game_.numCollectiblesAlive > 0)
      {
        completeText += collectiblesMissingText + '\n';
      }

      if (game_.mustReachFinish && !game_.reachedFinish)
      {
        completeText += reachFinishText + '\n';
      }

      text_.text = completeText;
    }
    else
    {
      text_.text = "You've completed the level!\n\nType 'next' to go to the Next level\nType 'menu' to go to the menu\nType a levels name (LevelX or TutorialX) to go to a specific level";
    }
  }

  public void Unfade(float duration)
  {
    from_ = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    to_ = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    elapsedTime_ = 0.0f;
    animationDuration_ = duration;
    animating_ = true;
  }
}
