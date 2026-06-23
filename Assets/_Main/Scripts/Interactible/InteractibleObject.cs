using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractibleObject : MonoBehaviour, IInteractable
{

    public string ActionName => (string.IsNullOrEmpty(ObjAction) ? "Use" : ObjAction);

    [SerializeField] private string ObjAction;
    [SerializeField] private ItemData requiredItem;

    [SerializeField] private string[] _refusalTextList;
    [SerializeField] private string[] _acceptanceTextList;

    [SerializeField] private AudioClip _refusalAudioClip;
    [SerializeField] private AudioClip _acceptanceAudioClip;

    [SerializeField] private string _refusalAnimTriggerName = "";
    
    private Collider _collider;
    private PlayerInventory _pInventory;
    private AudioSource _audioSource;
    private Animator _animator;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _pInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
    }

    public void Interact()
    {
        ItemData currentItem = _pInventory.GetCurrentlySelectedItem();
        if (_pInventory != null && currentItem != null && currentItem.ItemID == requiredItem.ItemID)
        {
            DisableCollider();
            AddTextSpeech(_acceptanceTextList);
            ShowInteraction(true);
            _pInventory.RemoveCurrentItem();
        }
        else
        {
            DisableCollider();
            AddTextSpeech(_refusalTextList, EnableCollider);
            ShowInteraction(false);
        }
    }

    private void AddTextSpeech(string[] textList, Action callBackAction = null)
    {
        if(textList != null)
        {
            Typewriter.EmptyMessageList();
            foreach (string text in textList)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    if (text == textList[^1])
                    {
                        Typewriter.AddMessage(text, callBackAction);
                    }
                    else
                    {
                        Typewriter.AddMessage(text);
                    }
                }
            }
            Typewriter.Activate();
        }
    }

    private void ShowInteraction(bool isAcceptance)
    {
        if (isAcceptance)
        {
            if(_audioSource != null && _acceptanceAudioClip != null)
            {
                _audioSource.PlayOneShot(_acceptanceAudioClip);
            }

            if(_animator != null)
            {
                _animator.SetBool("Status", true);
            }
        }
        else
        {
            if (_audioSource != null && _refusalAudioClip != null)
            {
                _audioSource.PlayOneShot(_refusalAudioClip);
            }

            if (_animator != null)
            {
                _animator.SetBool("Status", false);
                _animator.SetTrigger(_refusalAnimTriggerName);
            }
        }
    }

    private void EnableCollider()
    {
        if(_collider != null)
        {
            _collider.enabled = true;
        }
    }

    private void DisableCollider()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }

}
            
