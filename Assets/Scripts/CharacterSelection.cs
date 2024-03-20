using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier = 1.5f;
    [FormerlySerializedAs("dialogObject")] [SerializeField] private GameObject dialougeObject;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private int characterIndex;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite turnBackSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private static bool _isDialogueActive = false;
    private float _originalScale;
    private static readonly int GlowEffect = Shader.PropertyToID("_GlowEffect");

    private void Awake()
    {
        _originalScale = transform.localScale.x;
    }

    private void OnMouseEnter()
    {
        if (_isDialogueActive) return;
       
        transform.DOScale(scaleMultiplier * _originalScale, 0.5f);
    }
    
    private void OnMouseExit()
    {
        if (_isDialogueActive) return;
        
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
        dialogueManager.SetSpeaker(characterIndex);
        dialougeObject.SetActive(true);
        dialougeObject.GetComponent<DialogueRunner>().StartDialogue("Start");
        dialougeObject.GetComponent<DialogueRunner>().onDialogueComplete.AddListener(OnEndDialogue);
    }
    
    public void OnEndDialogue()
    {
        _isDialogueActive = false;
        
        dialougeObject.GetComponent<DialogueRunner>().onDialogueComplete.RemoveListener(OnEndDialogue);
    }

    public void StopGlow()
    {
        spriteRenderer.material.SetFloat(GlowEffect, 0);
    }

    public void MakeGlow()
    {
        spriteRenderer.material.SetFloat(GlowEffect, 1);
    }
}
