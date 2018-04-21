using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public delegate void WordListener(string str);

    public string currentInput
    {
        get
        {
            return currentInput_;
        }
    }

    private string currentInput_;
    private Dictionary<string, WordListener> words_;

	void Start ()
    {
        words_ = new Dictionary<string, WordListener>();
        currentInput_ = "";
    }
	
	void Update ()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // Check for backspace
            {
                if (currentInput_.Length != 0)
                {
                    currentInput_ = currentInput_.Remove(currentInput_.Length - 1, 1);
                }
            }
            else
            {
                currentInput_ += c;

                if (words_.ContainsKey(currentInput_))
                {
                    words_[currentInput_].Invoke(currentInput_);

                    currentInput_ = "";
                }
            }
        }
    }

    void RegisterWord(string word, WordListener listener)
    {
        if (words_.ContainsKey(word))
        {
            words_[word] += listener;
        }
        else
        {
            words_.Add(word, listener);
        }
    }
}
