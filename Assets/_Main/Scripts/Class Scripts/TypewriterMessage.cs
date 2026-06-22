using System;
using UnityEngine;

[System.Serializable]
public class TypewriterMessage
{
    private float _timer = 0;
    private int _charIndex = 0;
    private float _timePerChar = 0.05f;

    [SerializeField]
    public string currentMsg;
    private string _displayeMsg;

    private Action OnActionCallback;

    public TypewriterMessage(string msg, Action callbackAction = null)
    {
        currentMsg = msg;
        OnActionCallback = callbackAction;
    }

    public void Update()
    {
        if (string.IsNullOrEmpty(currentMsg)) return;

        _timer -= Time.deltaTime; //decrease the time so that the next character appears once the previous one is finished

        // Message Timer
        if(_timer <= 0)
        {
            _timer += _timePerChar;
            _charIndex++;

            _displayeMsg = currentMsg.Substring(0, _charIndex);
            _displayeMsg += "<color=#00000000>" + currentMsg.Substring(_charIndex) + "</color>";

            if(_charIndex >= currentMsg.Length)
            {
                Callback();
                currentMsg = null;
            }
        }
    }

    public bool isActive()
    {
        if (string.IsNullOrEmpty(currentMsg))
        {
            return false;
        }

        return _charIndex < currentMsg.Length;
    }

    public void Callback()
    {
        if(OnActionCallback != null)
        {
            OnActionCallback();
        }
    }

    public string CallbackAndMsgReturn()
    {
        if(OnActionCallback != null)
        {
            OnActionCallback();
        }

        return currentMsg;
    }

    public string GetFullMsg()
    {
        return currentMsg;
    }

    public string GetDisplayMsg()
    {
        return _displayeMsg;
    }

}
