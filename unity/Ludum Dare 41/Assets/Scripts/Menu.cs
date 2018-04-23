using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour 
{
  private TypeWriter typeWriter_;
  private FadeToBlack fadeToBlack_;
  
	void Start ()
  {
    typeWriter_ = GetComponent<TypeWriter>();
    typeWriter_.RegisterWord("start", WordListener);
    typeWriter_.RegisterWord("select", WordListener);
    typeWriter_.RegisterWord("level1", WordListener);
    typeWriter_.RegisterWord("level2", WordListener);
    typeWriter_.RegisterWord("level3", WordListener);
    typeWriter_.RegisterWord("level4", WordListener);
    typeWriter_.RegisterWord("level5", WordListener);
    typeWriter_.RegisterWord("level6", WordListener);

    fadeToBlack_ = GameObject.FindGameObjectWithTag("FadeToBlack").GetComponent<FadeToBlack>();
  }
  
  void Update ()
  {
		
	}

  void WordListener(string word)
  {
    switch (word)
    {
      case "start":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial01");
        break;
      case "select":
        ShowLevelSelect();
        break;
      case "level1":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
        break;
      case "level2":
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level02");
        break;
      case "level3":

        break;
      case "level4":

        break;
      case "level5":

        break;
      case "level6":

        break;
    }
  }

  void ShowLevelSelect()
  {
    
  }
}
