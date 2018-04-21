using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class WordVisualizer : MonoBehaviour
{
    public TypeWriter writer;

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

            //writer.RegisterWord("kappa", );
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
