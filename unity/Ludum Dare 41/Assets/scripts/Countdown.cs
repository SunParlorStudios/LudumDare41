using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Countdown : MonoBehaviour
{
  public delegate void CountdownFinishedListener();
  public CountdownFinishedListener CountdownFinishedEvent;

  private Text text_;

  private float countdown_ = 3.5f;

  private float animationTime_ = 0.0f;
  private float animationDuration_ = 0.3f;

  void Start()
  {
    text_ = GetComponent<Text>();
  }
  
  void Update()
  {
    countdown_ -= Time.deltaTime;

    if (countdown_ >= 2.5f)
    {
      if (text_.text != "3")
      {
        text_.text = "3";
        animationTime_ = 0.0f;
      }
    }
    else if (countdown_ >= 1.3f)
    {
      if (text_.text != "2")
      {
        text_.text = "2";
        animationTime_ = 0.0f;
      }
    }
    else if (countdown_ >= 0.0f)
    {
      if (text_.text != "1")
      {
        text_.text = "1";
        animationTime_ = 0.0f;
      }
    }
    else
    {
      if (text_.text != "GO!")
      {
        text_.text = "GO!";

        if (CountdownFinishedEvent != null)
        {
          CountdownFinishedEvent.Invoke();
        }
      }
    }

    animationTime_ += Time.deltaTime;
    text_.fontSize = (int)Mathf.Lerp(500, 100, Mathf.Clamp(animationTime_ / animationDuration_, 0, 1));
  }
}
