using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour, IInteractable
{
    public string ActionName => "Open Locker";

    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (GameManager.instance != null && GameManager.instance.GetKeyStatus())
        {
            UIManager.instance.StartPromptCoroutine(5, "You Win...");
            _collider.enabled = false;
        }
        else
        {
            UIManager.instance.StartPromptCoroutine(5, "Find The Key!");
        }
    }
}
            
