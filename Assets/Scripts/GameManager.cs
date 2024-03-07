using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
     public static GameManager instance { get; private set; }
     
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
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
            speaker2Transform.DOMove(newSpeaker2Position, 1f);
        }
}
