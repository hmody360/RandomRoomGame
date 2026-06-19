using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int NoOfRoomsToGenerate = 30;

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

    public int GetNoOfRoomsToGenerate()
    {
        return NoOfRoomsToGenerate;
    }
}
