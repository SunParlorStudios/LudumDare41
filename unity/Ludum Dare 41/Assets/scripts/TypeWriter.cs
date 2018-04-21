using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public delegate void WordListener(string str);

    private string currentInput;
    private Dictionary<string, WordListener> words;

	void Start ()
    {
        words = new Dictionary<string, WordListener>();
    }
	
	void Update ()
    {
        if (Input.inputString != "")
        {
            currentInput += Input.inputString;

            if(words.ContainsKey(currentInput))
            {
                words[currentInput].Invoke(currentInput);

                currentInput = "";
            }
        }
    }

    void RegisterWord(string word, WordListener listener)
    {
        if (words.ContainsKey(word))
        {
            words[word] += listener;
        }
        else
        {
            words.Add(word, listener);
        }
    }
}
