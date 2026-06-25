using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int NoOfRoomsToGenerate = 10;

    public static GameManager instance;
    [SerializeField] private bool _isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public int GetNoOfRoomsToGenerate()
    {
        return NoOfRoomsToGenerate;
    }

    public void SetNoOfRoomsToGenerate(int roomsNo)
    {
        NoOfRoomsToGenerate = roomsNo;
    }

    public void ToggleGamePause()
    {
        if (!_isGamePaused)
        {
            _isGamePaused = true;
            Time.timeScale = 0f;
            UIManager.instance.ShowPauseMenu();
        }
        else
        {
            _isGamePaused = false;
            Time.timeScale = 1f;
            UIManager.instance.HidePauseMenu();
        }
    }

    public void ResumeGame()
    {
        _isGamePaused = false;
        Time.timeScale = 1f;
        UIManager.instance.HidePauseMenu();
    }

    
}
