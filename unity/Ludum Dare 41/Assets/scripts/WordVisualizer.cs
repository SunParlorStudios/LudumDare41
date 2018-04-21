using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordVisualizer : MonoBehaviour
{
    public TypeWriter writer;

    public GameObject wordCompletionEffect;

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

            writer.anyWordCompletedEvent += WordReceiver;
        }
	}

    void WordReceiver(string word)
    {
        if (wordCompletionEffect != null)
        {
            GameObject effect = Instantiate(wordCompletionEffect, transform.parent);
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
