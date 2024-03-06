using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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
}
