using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinChecker : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other) //Check if the player is infront of the exit locker and has the key to trigger win, otherwise prompt to find key
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance != null && GameManager.instance.GetKeyStatus())
            {
                UIManager.instance.StartPromptCoroutine(5, "You Win...");
                _collider.enabled = false;
            }
        }
        else
        {
            UIManager.instance.StartPromptCoroutine(5, "Find The Key!");
        }
    }
}
