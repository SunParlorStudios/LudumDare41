using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  public Text timeLeftText;
  public Text timeLeftNumber;

  public Text enemiesLeftText;
  public Text enemiesLeftNumber;

  public Text powerCellsLeftText;
  public Text powerCellsLeftNumber;

  private Game game_;

	void Start ()
  {
    game_ = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();

    if (!game_.mustCollectAll)
    {
      powerCellsLeftText.enabled = false;
      powerCellsLeftNumber.enabled = false;

      enemiesLeftText.transform.position = powerCellsLeftText.transform.position;
      enemiesLeftNumber.transform.position = powerCellsLeftNumber.transform.position;
    }

    if (!game_.mustKillAll)
    {
      enemiesLeftText.enabled = false;
      enemiesLeftNumber.enabled = false;
    }

    if (!game_.mustCompleteInTime)
    {
      timeLeftText.enabled = false;
      timeLeftNumber.enabled = false;
    }
  }
	
	void Update ()
  {
    timeLeftNumber.text = (game_.timeLimit - game_.gameTime).ToString("N0");
    enemiesLeftNumber.text = (game_.numEnemiesAlive).ToString();
    powerCellsLeftNumber.text = (game_.numCollectiblesAlive).ToString();
  }
}
