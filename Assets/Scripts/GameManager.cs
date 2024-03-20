using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private List<String> dadJokes;
    [SerializeField] private DialogueRunner dialogueRunner;

    private int _currentGroup = 0;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void MakeCharacterLookBack(int characterIndex)
    {
        characters[characterIndex].GetComponent<CharacterSelection>().LookBack();
    }
    
    public void MakeCharacterLookForward(int characterIndex)
    {
        characters[characterIndex].GetComponent<CharacterSelection>().LookForward();
    }

    public void SwitchPlaces(int speaker1, int speaker2)
    {
        Transform speaker1Transform = characters[speaker1].transform;
        Transform speaker2Transform = characters[speaker2].transform;

        var speaker1Position = speaker1Transform.position;
        var speaker2Position = speaker2Transform.position;
        
        Vector3 newSpeaker1Position = 
            new Vector3(speaker2Position.x, speaker1Position.y, speaker1Position.z);
        Vector3 newSpeaker2Position =
            new Vector3(speaker1Position.x, speaker2Position.y, speaker2Position.z);
        
        speaker1Transform.DOMove(newSpeaker1Position, 1f);
        speaker1Transform.DOShakeRotation(1f, 5f, 10, 90);
        
        speaker2Transform.DOMove(newSpeaker2Position, 1f);
        speaker2Transform.DOShakeRotation(1f, 5f, 10, 90);
    }

    public void LoadNextGroup()
    {
        bool curGroupIsLast = (_currentGroup + 1) * 3 >= characters.Length;
        
        if (curGroupIsLast)
        {
            GetOffBus();
        }
        else
        {
            StartCoroutine(NextGroupFadeInOut());
        }
    }

    private void GetOffBus()
    {
        StartCoroutine(GetOffBusCoroutine());
    }

    private IEnumerator GetOffBusCoroutine()
    {
        PassCurGroup();
        
        yield return new WaitForSeconds(5f);
        
        BlackoutScreen();
        FadeInMessage("You got off the bus!\nGood Job!");
        
        yield return new WaitForSeconds(5f);
        
        FadeOutMessage();
        
        yield return new WaitForSeconds(5f);
        
        FadeInMessage("Press Space to restart");
        StartCoroutine(ListenForSpace());
    }

    private void PassCurGroup()
    {
        for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
        {
            characters[i].GetComponent<BoxCollider2D>().enabled = false;
            characters[i].GetComponent<CharacterSelection>().enabled = false;
            
            var curTransform = characters[i].transform;
            curTransform.DOScale(transform.localScale.x * 3, 5f);
            
            var relativeLocationOnScreen = Camera.main.WorldToViewportPoint(curTransform.position);

            float targetX;
            if (relativeLocationOnScreen.x < 0.5f)
            {
                targetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
                              curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
                
            }
            else
            {
                targetX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 
                              curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            }

            var curPosition = curTransform.position;
            curTransform.DOMove(new Vector3(targetX, curPosition.y - 6, curPosition.z), 5f);
            
            curTransform.DOShakeRotation(5f, 5f, 10, 90);
        }
    }

    private IEnumerator NextGroupFadeInOut()
    {
        PassCurGroup();
        
        //yield return new WaitForSeconds(0.5f);
        
        _currentGroup++;
        IntroduceGroup();
        
        yield return new WaitForSeconds(5f);

        for (int i = (_currentGroup - 1) * 3; i < (_currentGroup - 1) * 3 + 3; i++)
        {
            characters[i].SetActive(false);
        }
        
        for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
        {
            characters[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private void IntroduceGroup()
    {
        for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
        {
            characters[i].SetActive(true);
            characters[i].GetComponent<BoxCollider2D>().enabled = false;
            
            var curTransform = characters[i].transform;
            var targetScale = curTransform.localScale.x;
            
            curTransform.localScale = Vector3.one * (targetScale * 0.1f);
            curTransform.DOScale(targetScale, 5f);
            
            var targetPos = curTransform.position;
            curTransform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            curTransform.DOMove(targetPos, 5f);
            
            curTransform.DOShakeRotation(5f, 5f, 10, 90);
        }
    }

    public void BeatUpEnding()
    {
        StartCoroutine(BeatUpEndingCoroutine());
    }
    
    private IEnumerator BeatUpEndingCoroutine()
    {
        BlackoutScreen();
        FadeInMessage("You were beat up!\nGame Over!");
        yield return new WaitForSeconds(5f);
        FadeOutMessage();
        yield return new WaitForSeconds(5f);
        FadeInMessage("Press Space to restart");
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(ListenForSpace());
    }
    
    private IEnumerator ListenForSpace()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
                yield break;
            }
            yield return null;
        }
    }
    
    private void BlackoutScreen()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = new Color(0, 0, 0, 0);
        blackScreen.DOFade(1, 5f);
    }
    
    private void FadeInMessage(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        messageText.color = new Color(1, 1, 1, 0);
        DOTween.Play(messageText.DOColor(new Color(1, 1, 1, 1), 5f));
    }
    
    private void FadeOutMessage()
    {
        messageText.DOFade(0, 5f);
    }

    public void CharacterLeave(int speaker)
    {
        StartCoroutine(ScurryOffScreen(speaker));
    }
    
    private IEnumerator ScurryOffScreen(int speaker)
    {
        var curTransform = characters[speaker].transform;
        
        var relativeLocationOnScreen = Camera.main.WorldToViewportPoint(curTransform.position);
        
        float targetX;
        if (relativeLocationOnScreen.x < 0.5f)
        {
            targetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
                          curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }
        else
        {
            targetX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 
                          curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        }
        
        Vector3 curPosition = curTransform.position;
        curTransform.DOMove(new Vector3(targetX, curPosition.y, curPosition.z), 5f);
        curTransform.DOShakeRotation(5f, 5f, 10, 90);
        
        curTransform.GetComponent<SpriteRenderer>().flipX = true;
        
        yield return new WaitForSeconds(5f);
        
        characters[speaker].SetActive(false);
    }

    public void MakeCharacterGlow(int characterIndex)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].GetComponent<CharacterSelection>().StopGlow();
        }
        characters[characterIndex].GetComponent<CharacterSelection>().MakeGlow();
    }
    
    public void MakeCharactersStopGlow()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].GetComponent<CharacterSelection>().StopGlow();
        }
    }

    public string GetRandomDadJoke()
    {
        int randomIndex = UnityEngine.Random.Range(0, dadJokes.Count);
        return dadJokes[randomIndex];
    }

    public DialogueRunner GetDialogueRunner()
    {
        return dialogueRunner;
    }
}
