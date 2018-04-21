using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
  public float rotationSpeed;

  [Range(0.0f, 1.0f)]
  public float moveStepSize;

  public Canvas canvas;

  private Transform target_;
  private RectTransform canvasTransform_;
  private RectTransform rect_;

  void Start()
  {
    gameObject.SetActive(false);
    canvasTransform_ = canvas.GetComponent<RectTransform>();
    rect_ = GetComponent<RectTransform>();
  }

  public void SetTarget(Transform target)
  {
    target_ = target;

    if (target_ == null)
    {
      gameObject.SetActive(false);
      return;
    }

    gameObject.SetActive(true);
  }
  
	void Update()
  {
    transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}

  void FixedUpdate()
  {
    if (target_ == null)
    {
      return;
    }

    Vector2 vpPos = Camera.main.WorldToViewportPoint(target_.position);
    Vector2 delta = canvasTransform_.sizeDelta;

    Vector2 screenPos = new Vector2(vpPos.x * delta.x, vpPos.y * delta.y);
    screenPos -= delta * 0.5f;
    
    rect_.localPosition = Vector2.Lerp(rect_.localPosition, screenPos, moveStepSize);
  }
}
