using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordFailEffect : MonoBehaviour
{
    public float duration = 1.0f;
    public int numOscillations = 5;
    public float oscillationIntensity = 10.0f;

    private Vector3[] oscillationPoints_;
    private int oscillations_;
    private float oscillationDuration_;

    private Vector3 originalPosition_;
    private float playbackTime_;
    private float startOpacity_;
    private float endOpacity_;
    private Text text_;

    void Start()
    {
        playbackTime_ = 0.0f;

        originalPosition_ = transform.localPosition;

        oscillationPoints_ = new Vector3[2];

        oscillationPoints_[0] = transform.localPosition + Vector3.left * oscillationIntensity;
        oscillationPoints_[1] = transform.localPosition + Vector3.right * oscillationIntensity;

        oscillations_ = 0;
        oscillationDuration_ = duration / numOscillations;

        startOpacity_ = 1.0f;
        endOpacity_ = 0.0f;

        text_ = GetComponent<Text>();
    }

    void Update()
    {
        playbackTime_ += Time.deltaTime;

        Vector3 oscillateFrom, oscillateTo;

        oscillations_ = Mathf.FloorToInt((playbackTime_ / duration) * numOscillations);

        if (oscillations_ == 0)
        {
            oscillateFrom = originalPosition_;
            oscillateTo = oscillationPoints_[0];
        }
        else if (oscillations_ >= numOscillations - 1)
        {
            oscillateFrom = oscillationPoints_[numOscillations % 2];
            oscillateTo = originalPosition_;
        }
        else
        {
            oscillateFrom = oscillationPoints_[(oscillations_ - 1) % 2];
            oscillateTo = oscillationPoints_[oscillations_ % 2];
        }

        transform.localPosition = Vector3.Lerp(oscillateFrom, oscillateTo, (playbackTime_ % oscillationDuration_) / oscillationDuration_);

        text_.color = new Color(
            text_.color.r,
            text_.color.g,
            text_.color.b,
            Mathf.Lerp(startOpacity_, endOpacity_, playbackTime_ / duration)
        );

        if (playbackTime_ >= duration)
        {
            Destroy(gameObject);
        }
    }
}
