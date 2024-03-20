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
    
    [YarnFunction("getDadJoke")]
    public static string getDadJoke()
    {
        return GameManager.instance.getRandomeDadJoke();
    }
    
    [YarnCommand("SpeakerLookBack")]
    public static void SpeakerLookForward()
    {
        GameManager.instance.MakeCharacterLookBack(_currentSpeaker);
    }
    
    [YarnCommand("SpeakerLookForward")]
    public static void SpeakerLookBack()
    {
        GameManager.instance.MakeCharacterLookForward(_currentSpeaker);
    }
    
    [YarnCommand("switchPlaces")]
    public static void switchPlaces(int speaker1, int speaker2)
    {
        GameManager.instance.SwitchPlaces(speaker1, speaker2);
    }
    
    [YarnCommand("loadNextGroup")]
    public static void loadNextGroup()
    {
        GameManager.instance.LoadNextGroup();
    }
    
    [YarnCommand("lookForward")]
    public static void lookForward(int speaker)
    {
        GameManager.instance.MakeCharacterLookForward(speaker);
    }
    
    [YarnCommand("lookBack")]
    public static void lookBack(int speaker)
    {
        GameManager.instance.MakeCharacterLookBack(speaker);
    }

    [YarnCommand("beatUpEnding")]
    public static void beatUpEnding()
    {
        GameManager.instance.BeatUpEnding();
    }

    [YarnCommand("characterLeave")]
    public static void characterLeave(int speaker)
    {
        GameManager.instance.CharacterLeave(speaker);
    }
}
