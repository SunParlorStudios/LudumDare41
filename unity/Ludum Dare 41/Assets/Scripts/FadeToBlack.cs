using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
  private Image image_;

  private Color from_ = new Color(0.0f, 0.0f, 0.0f, 1.0f);
  private Color to_ = new Color(0.0f, 0.0f, 0.0f, 0.0f);

  private float animationDuration_ = 0.0f;
  private float elapsedTime_ = 0.0f;
  private bool animating_ = false;

	void Start ()
  {
    image_ = GetComponent<Image>();
	}
	
	void Update ()
  {
		if (animating_)
    {
      elapsedTime_ += Time.deltaTime;
      float t = elapsedTime_ / animationDuration_;

      image_.color = Color.Lerp(from_, to_, t);

      if (t >= 1.0f)
      {
        animating_ = false;
      }
    }
	}

  public void Fade(float duration)
  {
    from_ = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    to_ = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    elapsedTime_ = 0.0f;
    animationDuration_ = duration;
    animating_ = true;
  }

  public void Unfade(float duration)
  {
    from_ = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    to_ = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    elapsedTime_ = 0.0f;
    animationDuration_ = duration;
    animating_ = true;
  }
}
