using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Yarn.Unity;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private GameObject dialogObject;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int characterIndex;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite turnBackSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    
    private static bool _isDialogueActive = false;
    
    private float _originalScale;
    private void Start()
    {
        _originalScale = transform.localScale.x;
    }

    private void OnMouseEnter()
    {
        if (_isDialogueActive) return;
        Debug.Log("Mouse entered");
        // Enlarge the character with dotween
        transform.DOScale(scaleMultiplier * _originalScale, 0.5f);
    }
    
    private void OnMouseExit()
    {
        if (_isDialogueActive) return;
        // Return the character to its original size with dotween
        transform.DOScale(_originalScale, 0.5f);
    }
    
    public void LookBack()
    {
        spriteRenderer.sprite = turnBackSprite;
    }
    
    public void LookForward()
    {
        spriteRenderer.sprite = idleSprite;
    }
    
    private void OnMouseDown()
    {
        if (_isDialogueActive) return;
        
        _isDialogueActive = true;
        transform.DOScale(_originalScale * 0.9f, 0.25f);
        transform.DOScale(_originalScale, 0.25f).SetDelay(0.25f);
        dialogueManager.setSpeaker(characterIndex);
        dialogObject.SetActive(true);
        dialogObject.GetComponent<DialogueRunner>().StartDialogue("Start");
        dialogObject.GetComponent<DialogueRunner>().onDialogueComplete.AddListener(EndDialogue);
        //spriteRenderer.sprite = turnBackSprite;
    }
    
    public void EndDialogue()
    {
        _isDialogueActive = false;
        //spriteRenderer.sprite = idleSprite;
        dialogObject.GetComponent<DialogueRunner>().onDialogueComplete.RemoveListener(EndDialogue);
    }
}
