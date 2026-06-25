using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuContainer;
    [SerializeField] private GameObject OptionsMenuContainer;

    [SerializeField] private Slider _roomNoSlider;
    [SerializeField] private TextMeshProUGUI _roomsCounter;

    public static MainMenuUIManager instance;

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
        if (_roomNoSlider != null)
        {
            int roomNo = PlayerPrefs.GetInt("RoomNo", 10);
            _roomNoSlider.value = roomNo;
            SetRoomNo(roomNo);
            _roomNoSlider.onValueChanged.AddListener(SetRoomNo);
        }
    }

    private void SetRoomNo(float roomNo)
    {
        int value = Mathf.RoundToInt(roomNo);
        PlayerPrefs.SetInt("RoomNo", value);
        if(GameManager.instance != null)
        {
            GameManager.instance.SetNoOfRoomsToGenerate(value);
        } 
        UpdateRoomsCounter(value);
    }

    public void ShowOptionsMenu()
    {
        MainMenuContainer.SetActive(false);
        OptionsMenuContainer.SetActive(true);
    }

    public void ShowMainMenu()
    {
        OptionsMenuContainer.SetActive(false);
        MainMenuContainer.SetActive(true);
    }

    public void UpdateRoomsCounter(int NoOfRooms)
    {
        if(_roomsCounter == null)
        {
            return;
        }

        _roomsCounter.text = NoOfRooms.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
