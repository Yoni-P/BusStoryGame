using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    private static int _currentSpeaker = 0;

    public void SetSpeaker(int speaker)
    {
        _currentSpeaker = speaker;
    }
    
    [YarnFunction("setSpeaker")]
    public static int UpdateSpeaker()
    {
        return _currentSpeaker;
    }
    
    [YarnFunction("getDadJoke")]
    public static string GetDadJoke()
    {
        return GameManager.Instance.GetRandomDadJoke();
    }
    
    [YarnCommand("SpeakerLookBack")]
    public static void SpeakerLookForward()
    {
        GameManager.Instance.MakeCharacterLookBack(_currentSpeaker);
    }
    
    [YarnCommand("SpeakerLookForward")]
    public static void SpeakerLookBack()
    {
        GameManager.Instance.MakeCharacterLookForward(_currentSpeaker);
    }
    
    [YarnCommand("switchPlaces")]
    public static void SwitchPlaces(int speaker1, int speaker2)
    {
        GameManager.Instance.SwitchPlaces(speaker1, speaker2);
    }
    
    [YarnCommand("loadNextGroup")]
    public static void LoadNextGroup()
    {
        GameManager.Instance.LoadNextGroup();
    }
    
    [YarnCommand("lookForward")]
    public static void LookForward(int speaker)
    {
        GameManager.Instance.MakeCharacterLookForward(speaker);
    }
    
    [YarnCommand("lookBack")]
    public static void LookBack(int speaker)
    {
        GameManager.Instance.MakeCharacterLookBack(speaker);
    }

    [YarnCommand("beatUpEnding")]
    public static void BeatUpEnding()
    {
        GameManager.Instance.BeatUpEnding();
    }

    [YarnCommand("characterLeave")]
    public static void CharacterLeave(int speaker)
    {
        GameManager.Instance.CharacterLeave(speaker);
    }
}
