using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private DialogueRunner _dialogueRunner;
     private static int _currentSpeaker = 0;

     private void Start()
     {
            
     }

     public void setSpeaker(int speaker)
    {
        _currentSpeaker = speaker;
    }
    
    [YarnFunction("setSpeaker")]
    public static int updateSpeaker()
    {
        return _currentSpeaker;
    }
}