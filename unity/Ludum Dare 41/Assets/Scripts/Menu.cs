using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour 
{
  private TypeWriter typeWriter_;
  private FadeToBlack fadeToBlack_;

  public GameObject levelSelectObject;
  private bool animating_;
  
	void Start ()
  {
    typeWriter_ = GetComponent<TypeWriter>();
    typeWriter_.RegisterWord("start", WordListener);
    typeWriter_.RegisterWord("select", WordListener);
    typeWriter_.RegisterWord("menu", WordListener);
    typeWriter_.RegisterWord("tutorial1", WordListener);
    typeWriter_.RegisterWord("tutorial2", WordListener);
    typeWriter_.RegisterWord("tutorial3", WordListener);
    typeWriter_.RegisterWord("tutorial4", WordListener);
    typeWriter_.RegisterWord("tutorial5", WordListener);
    typeWriter_.RegisterWord("level1", WordListener);
    typeWriter_.RegisterWord("level2", WordListener);
    typeWriter_.RegisterWord("level3", WordListener);
    typeWriter_.RegisterWord("level4", WordListener);
    typeWriter_.RegisterWord("level5", WordListener);

    fadeToBlack_ = GameObject.FindGameObjectWithTag("FadeToBlack").GetComponent<FadeToBlack>();
  }
  
  void Update ()
  {
		if (animating_)
    {
      levelSelectObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(levelSelectObject.GetComponent<RectTransform>().anchoredPosition, Vector3.zero, 0.03f);
    }
    else
    {
      levelSelectObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(levelSelectObject.GetComponent<RectTransform>().anchoredPosition, new Vector3(800, 0, 0), 0.03f);
    }
	}

  void WordListener(string word)
  {
    switch (word)
    {
      case "select":
        animating_ = true;
        break;
      case "menu":
        animating_ = false;
        break;
      case "start":
      case "tutorial1":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial01");
        break;
      case "tutorial2":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial02");
        break;
      case "tutorial3":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial03");
        break;
      case "tutorial4":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial04");
        break;
      case "tutorial5":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial05");
        break;
      case "level1":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
        break;
      case "level2":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level02");
        break;
      case "level3":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level03");
        break;
      case "level4":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level04");
        break;
      case "level5":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level05");
        break;
    }
  }

  void ShowLevelSelect()
  {
    animating_ = true;
  }
}
