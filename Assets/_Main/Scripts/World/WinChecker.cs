using UnityEngine;

public class WinChecker : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
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
