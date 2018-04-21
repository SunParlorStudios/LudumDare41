using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordVisualizer : MonoBehaviour
{
    public TypeWriter writer;

    public GameObject wordSuccessEffect;
    public GameObject wordFailEffect;

    private Text text;

	void Start ()
    {
		if (!writer)
        {
            enabled = false;
        }
        else
        {
            text = GetComponent<Text>();
            writer.newCharacterEvent += NewCharacterReceiver;

            writer.anyWordCompletedEvent += WordSuccessReceiver;
            writer.resetEvent += WordFailReceiver;
        }
    }

    void WordSuccessReceiver(string word)
    {
        if (wordSuccessEffect != null)
        {
            GameObject effect = Instantiate(wordSuccessEffect, transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<Text>().text = word;
        }
    }

    void WordFailReceiver(string word)
    {
        if (wordFailEffect != null)
        {
            GameObject effect = Instantiate(wordFailEffect, transform.parent);
            effect.transform.position = transform.position;
            effect.GetComponent<Text>().text = word;
        }
    }

    void NewCharacterReceiver(char c, string preInput, string postInput)
    {
        text.text = postInput;
    }
	
	void Update ()
    {
		
	}
}
