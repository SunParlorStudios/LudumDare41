﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
  public delegate void NewCharacterListener(char character, string preInput, string postInput);
  public delegate void ResetListener(string errorWord);
  public delegate void WordListener(string word);

  public ResetListener resetEvent;
  public NewCharacterListener newCharacterEvent;
  public WordListener anyWordCompletedEvent;

  public bool allowInput = false;

  public string currentInput
  {
    get
    {
      return currentInput_;
    }
  }

  private string currentInput_;
  private Dictionary<string, WordListener> words_;
  private AudioSource audio_;

  void Awake()
  {
    words_ = new Dictionary<string, WordListener>();
    currentInput_ = "";

    audio_ = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (allowInput)
    {
      foreach (char c in Input.inputString)
      {
        audio_.Play();

        string preInput = currentInput_;
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
            if (entry.Key.Substring(0, Mathf.Clamp(currentInput_.Length, 0, entry.Key.Length)) == currentInput_)
            {
              foundMatchingWord = true;
              break;
            }
          }

          if (!foundMatchingWord)
          {
            if (resetEvent != null)
            {
              resetEvent.Invoke(currentInput_);
            }

            currentInput_ = "";
          }
          else
          {
            if (words_.ContainsKey(currentInput_))
            {
              if (anyWordCompletedEvent != null)
              {
                anyWordCompletedEvent.Invoke(currentInput_);
              }

              words_[currentInput_].Invoke(currentInput_);
              currentInput_ = "";
            }
          }
        }

        if (newCharacterEvent != null)
        {
          newCharacterEvent.Invoke(c, preInput, currentInput_);
        }
      }
    }
  }

  public void RegisterWord(string word, WordListener listener)
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
