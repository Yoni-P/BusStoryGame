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
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Image blackScreen;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private List<String> dadJokes;
    [SerializeField] private DialogueRunner dialogueRunner;

    private int _currentGroup = 0;
     public static GameManager instance { get; private set; }
     
        private void Awake()
        {
            instance = this;
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
            
            Vector3 newSpeaker1Position = 
                new Vector3(speaker2Transform.position.x, speaker1Transform.position.y, speaker1Transform.position.z);
            Vector3 newSpeaker2Position =
                new Vector3(speaker1Transform.position.x, speaker2Transform.position.y, speaker2Transform.position.z);


            speaker1Transform.DOMove(newSpeaker1Position, 1f);
            speaker1Transform.DOShakeRotation(1f, 5f, 10, 90);
            speaker2Transform.DOMove(newSpeaker2Position, 1f);
            speaker2Transform.DOShakeRotation(1f, 5f, 10, 90);
        }

        public void LoadNextGroup()
        {
            if ((_currentGroup + 1) * 3 >= characters.Length)
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
            for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
            {
                characters[i].GetComponent<BoxCollider2D>().enabled = false;
                characters[i].GetComponent<CharacterSelection>().enabled = false;
                var curTransform = characters[i].transform;
                curTransform.DOScale(transform.localScale.x * 3, 5f);
                var realtiveLocationOnScreen = Camera.main.WorldToViewportPoint(curTransform.position);
                if (realtiveLocationOnScreen.x < 0.5f)
                {
                    var targetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
                                  curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
                    curTransform.DOMove(new Vector3(targetX, curTransform.position.y - 6, curTransform.position.z), 5f);
                }
                else
                {
                    var targetX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 
                                  curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    curTransform.DOMove(new Vector3(targetX, curTransform.position.y - 6, curTransform.position.z), 5f);
                }
                characters[i].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 5f);
                curTransform.DOShakeRotation(5f, 5f, 10, 90);
            }
            yield return new WaitForSeconds(5f);
            BlackoutScreen();
            FadeInMessage("You got off the bus!\nGood Job!");
            yield return new WaitForSeconds(5f);
            FadeOutMessage();
            yield return new WaitForSeconds(5f);
            FadeInMessage("Press Space to restart");
            StartCoroutine(ListenForSpace());
        }

        private IEnumerator NextGroupFadeInOut()
        {
            for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
            {
                characters[i].GetComponent<BoxCollider2D>().enabled = false;
                characters[i].GetComponent<CharacterSelection>().enabled = false;
                var curTransform = characters[i].transform;
                curTransform.DOScale(transform.localScale.x * 3, 5f);
                var realtiveLocationOnScreen = Camera.main.WorldToViewportPoint(curTransform.position);
                if (realtiveLocationOnScreen.x < 0.5f)
                {
                    var targetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
                                  curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
                    curTransform.DOMove(new Vector3(targetX, curTransform.position.y - 6, curTransform.position.z), 5f);
                }
                else
                {
                    var targetX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 
                                  curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    curTransform.DOMove(new Vector3(targetX, curTransform.position.y - 6, curTransform.position.z), 5f);
                }
                characters[i].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 5f);
                curTransform.DOShakeRotation(5f, 5f, 10, 90);
            }
            yield return new WaitForSeconds(0.5f);
            _currentGroup++;
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
                characters[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                characters[i].GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 5f);
                curTransform.DOShakeRotation(5f, 5f, 10, 90);
            }
            yield return new WaitForSeconds(5f);
            characters[(_currentGroup-1) * 3].SetActive(false);
            characters[(_currentGroup-1) * 3 + 1].SetActive(false);
            characters[(_currentGroup-1) * 3 + 2].SetActive(false);
            for (int i = _currentGroup * 3; i < _currentGroup * 3 + 3; i++)
            {
                characters[i].GetComponent<BoxCollider2D>().enabled = true;
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
            var realtiveLocationOnScreen = Camera.main.WorldToViewportPoint(curTransform.position);
            if (realtiveLocationOnScreen.x < 0.5f)
            {
                var targetX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - 
                              curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2; 
                curTransform.DOMove(new Vector3(targetX, curTransform.position.y, curTransform.position.z), 5f);
            }
            else
            {
                var targetX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + 
                              curTransform.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                curTransform.DOMove(new Vector3(targetX, curTransform.position.y, curTransform.position.z), 5f);
            }
            curTransform.GetComponent<SpriteRenderer>().flipX = true;
            curTransform.DOShakeRotation(5f, 5f, 10, 90);
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

        public string getRandomeDadJoke()
        {
            int randomIndex = UnityEngine.Random.Range(0, dadJokes.Count);
            return dadJokes[randomIndex];
        }

        public DialogueRunner GetDialogueRunner()
        {
            return dialogueRunner;
        }
}
