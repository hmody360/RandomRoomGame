using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool isKeyObtained = false;

    public static GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleKeyObtained() //Prompt Key Collection
    {
        isKeyObtained = true;
    }

    public bool GetKeyStatus() //Check Status of Key
    {
        return isKeyObtained;
    }
}
