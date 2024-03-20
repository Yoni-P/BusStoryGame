using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Markup;
using Yarn.Unity;

public class SpeechBubble : DialogueViewBase
{
    [SerializeField] private GameObject PlayerSpeechBubble;
    [SerializeField] private GameObject speechBubbleA;
    [SerializeField] private GameObject speechBubbleB;
    [SerializeField] private GameObject speechBubbleC;
    [SerializeField] private GameObject speechBubbleD;
    [SerializeField] private GameObject speechBubbleE;
    [SerializeField] private GameObject speechBubbleF;
    
    private Dictionary<String, GameObject> nameToSpeechBubble = new Dictionary<string, GameObject>();
    private LocalizedLine curDialogueLine;
    private GameObject curSpeechBubble;

    private void Start()
    {
        nameToSpeechBubble.Add("P", PlayerSpeechBubble);
        nameToSpeechBubble.Add("A", speechBubbleA);
        nameToSpeechBubble.Add("B", speechBubbleB);
        nameToSpeechBubble.Add("C", speechBubbleC);
        nameToSpeechBubble.Add("D", speechBubbleD);
        nameToSpeechBubble.Add("E", speechBubbleE);
        nameToSpeechBubble.Add("F", speechBubbleF);
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        //base.RunLine(dialogueLine, onDialogueLineFinished);
        if (dialogueLine == null) return;
        curDialogueLine = dialogueLine;
        curSpeechBubble = GetCurSpeechBubble(dialogueLine.CharacterName);
        if (curSpeechBubble != null)
        {
            ActivateCurSpeechBubble(dialogueLine.CharacterName);
            ActivateGlow(dialogueLine.CharacterName);
            curSpeechBubble.GetComponentInChildren<TextMeshProUGUI>().text = dialogueLine.TextWithoutCharacterName.Text;
        }

        onDialogueLineFinished += GameManager.Instance.MakeCharactersStopGlow;
        StartCoroutine(InnerRunLine(dialogueLine, onDialogueLineFinished));
    }

    private void ActivateGlow(string dialogueLineCharacterName)
    {
        if (dialogueLineCharacterName is null or "P") return;
        GameManager.Instance.MakeCharacterGlow(dialogueLineCharacterName[0] % 'A');
    }

    private IEnumerator InnerRunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        bool isLastLine = dialogueLine.Metadata != null && dialogueLine.Metadata.Contains("last");
        bool isLineBeforeOption = dialogueLine.Metadata != null && dialogueLine.Metadata.Contains("lastLine");
        // debug log the metadata by joining the list of strings
        if (dialogueLine.Metadata != null) Debug.Log(string.Join(", ", dialogueLine.Metadata));
        
        if (isLastLine || dialogueLine.Metadata == null) yield break;
        yield return new WaitForSeconds(2);
        onDialogueLineFinished?.Invoke();
    }

    public override void InterruptLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        Debug.Log("Interrupting line");
        if (dialogueLine == null) return;
        StopAllCoroutines();
        onDialogueLineFinished?.Invoke();
    }

    public override void DismissLine(Action onDismissalComplete)
    {
        if (curDialogueLine == null) return;
        curDialogueLine = null;
        onDismissalComplete?.Invoke();
    }

    public override void DialogueComplete()
    {
        GameManager.Instance.MakeCharactersStopGlow();
        if (curSpeechBubble != null)
        {
            curSpeechBubble.SetActive(false);
            curSpeechBubble = null;
        }
        curDialogueLine = null;
    }

    private void ActivateCurSpeechBubble(string dialogueLineCharacterName)
    {
        foreach (GameObject speechBubble in nameToSpeechBubble.Values)
        {
            speechBubble.SetActive(false);
        }
        GameObject curSpeechBubble = GetCurSpeechBubble(dialogueLineCharacterName);
        curSpeechBubble.SetActive(true);
    }

    private GameObject GetCurSpeechBubble(String characterName)
    {
        GameObject curSpeechBubble = null;
        if (characterName != null && nameToSpeechBubble.TryGetValue(characterName, out var value))
        {
            curSpeechBubble = value;
        }
        return curSpeechBubble;
    }

    public override void UserRequestedViewAdvancement()
    {
        //base.UserRequestedViewAdvancement();
        if (curDialogueLine == null) return;
        requestInterrupt?.Invoke();
    }
}
