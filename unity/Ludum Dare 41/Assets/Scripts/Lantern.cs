using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
  public Color flareColor;
  public Color embersColor;
  public Color flameColor;

  public LightFlare flare;
  public Light pointLight;
  public ParticleSystem embers;
  public ParticleSystem flame;
  
	void Awake()
  {
    SetColors();
	}

  void SetColors()
  {
    flare.OverrideColor(flareColor);

    ParticleSystem.MainModule main = embers.main;
    main.startColor = embersColor;

    main = flame.main;
    main.startColor = flameColor;
    pointLight.color = flameColor;
  }

  void FixedUpdate()
  {
    SetColors();
  }
}
