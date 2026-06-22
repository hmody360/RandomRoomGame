using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;

    private static Typewriter _instance;
    private List<TypewriterMessage> _msgList = new List<TypewriterMessage>();

    private TypewriterMessage _currentMsg = null;
    private int _msgIndex = 0;

    public static event Action OnMessageDisplay;
    public static event Action OnMessageStop;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(_msgList.Count > 0 && _currentMsg != null)
        {
            _currentMsg.Update();
            TextComponent.text = _currentMsg.GetDisplayMsg();
        }
    }

    public static void AddMessage(string msg, Action callbackAction = null)
    {
        TypewriterMessage typeWriterMsg = new TypewriterMessage(msg, callbackAction);
        _instance._msgList.Add(typeWriterMsg);
    }

    public static void Activate()
    {
        Debug.Log("typewriter being activated");
        _instance._currentMsg = _instance._msgList[0];
        UIManager.instance.ShowSpeechBubble();
        OnMessageDisplay?.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public static void EmptyMessageList()
    {
        _instance._msgList.Clear();
    }

    public void WriteNextMessageInQueue()
    {
        if(_currentMsg != null && _currentMsg.isActive())
        {
            TextComponent.text = _currentMsg.CallbackAndMsgReturn();
            _currentMsg = null;
            //UIManager.instance.HideSpeechBubble();
            //OnMessageStop?.Invoke();
            //Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        _msgIndex++;

        if(_msgIndex >= _msgList.Count)
        {
            _currentMsg = null;
            TextComponent.text = "";
            UIManager.instance.HideSpeechBubble();
            OnMessageStop?.Invoke();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        _currentMsg = _msgList[_msgIndex];
    }
}
