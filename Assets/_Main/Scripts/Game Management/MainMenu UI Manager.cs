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

    [SerializeField] private Slider _renderDistanceSlider;
    [SerializeField] private TextMeshProUGUI _renderDistanceCounter;

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

        if (_renderDistanceSlider != null)
        {
            int renderDistance = PlayerPrefs.GetInt("RenderDistance", 2);
            _renderDistanceSlider.value = renderDistance;
            SetRenderDistance(renderDistance);
            _renderDistanceSlider.onValueChanged.AddListener(SetRenderDistance);
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

    private void SetRenderDistance(float renderDistance)
    {
        int value = Mathf.RoundToInt(renderDistance);
        PlayerPrefs.SetInt("RenderDistance", value);
        if (GameManager.instance != null)
        {
            GameManager.instance.SetRenderDistance(value);
        }
        UpdateRenderDistanceCounter(value);
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

    public void UpdateRenderDistanceCounter(int RenderDistance)
    {
        if(_renderDistanceCounter == null)
        {
            return;
        }

        _renderDistanceCounter.text = RenderDistance.ToString();
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
