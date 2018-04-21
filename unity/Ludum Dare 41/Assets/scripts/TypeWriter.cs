﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public delegate void NewCharacterListener(char character);
    public delegate void ResetListener();
    public delegate void WordListener(string word);

    public string currentInput
    {
        get
        {
            return currentInput_;
        }
    }
    
    private string currentInput_;
    private Dictionary<string, WordListener> words_;
    private ResetListener resetListener_;
    private NewCharacterListener newCharacterListener_;

	void Start ()
    {
        words_ = new Dictionary<string, WordListener>();
        currentInput_ = "";
    }
	
	void Update ()
    {
        foreach (char c in Input.inputString)
        {
            newCharacterListener_.Invoke(c);
            if (c == '\b')
            {
                if (currentInput_.Length != 0)
                {
                    currentInput_ = currentInput_.Remove(currentInput_.Length - 1, 1);
                }
            }
            else
            {
                currentInput_ += c;

                bool foundMatchingWord = false;
                foreach (KeyValuePair<string, WordListener> entry in words_)
                {
                    if (entry.Key.Substring(0, currentInput_.Length) == currentInput_)
                    {
                        foundMatchingWord = true;
                        break;
                    }
                }

                if (!foundMatchingWord)
                {
                    resetListener_.Invoke();
                    currentInput_ = "";
                }
                else
                {
                    if (words_.ContainsKey(currentInput_))
                    {
                        words_[currentInput_].Invoke(currentInput_);
                        currentInput_ = "";
                    }
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
