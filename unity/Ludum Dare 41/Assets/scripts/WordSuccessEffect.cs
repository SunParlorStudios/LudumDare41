using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordSuccessEffect : MonoBehaviour
{
    public float duration = 0.3f;
    private float playbackTime_;
    private Vector3 startScale_;
    private Vector3 endScale_;
    private float startOpacity_;
    private float endOpacity_;
    private Text text_;

	void Start ()
    {
        playbackTime_ = 0.0f;
        startScale_ = transform.localScale;
        endScale_ = transform.localScale * 2.0f;
        startOpacity_ = 1.0f;
        endOpacity_ = 0.0f;

        text_ = GetComponent<Text>();
	}
	
	void Update ()
    {
        playbackTime_ += Time.deltaTime;

        transform.localScale = Vector3.Lerp(startScale_, endScale_, playbackTime_ / duration);
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
