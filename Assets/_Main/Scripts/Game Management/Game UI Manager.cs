using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Speech UI Elements")]
    [SerializeField] private GameObject _speechContainer;

    [Header("Interact UI Elements")]
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private GameObject _interactContainer;

    [Header("Inventory UI Elements")]
    [SerializeField] private Sprite _normalSlot;
    [SerializeField] private Sprite _selectedSlot;
    [SerializeField] private Image[] _ImageSlotList;
    [SerializeField] private Image[] _ImageIconList;

    [Header("Menu UI Elements")]
    [SerializeField] private GameObject _HUDUIContainer;
    [SerializeField] private GameObject _PauseMenuContainer;
    [SerializeField] private GameObject _PauseMenuElements;
    [SerializeField] private GameObject _OptionsMenuContainer;


    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Prompt Section
    public void ShowSpeechBubble() //Show Speech Bubble UI
    {
        _speechContainer.SetActive(true);
    }

    public void HideSpeechBubble() //Set Text To Blank and Hide it
    {
        _speechContainer.SetActive(false);
    }

    // Interaction Section
    public void ShowInteractText(string text)
    {
        _interactText.text = text;
        _interactContainer.SetActive(true);
    }

    public void HideInteractText()
    {
        _interactText.text = "";
        _interactContainer.SetActive(false);
    }



    // Inventory Section

    public void SelectItemSlot(int slot)
    {
        if (slot >= 0 && slot < _ImageSlotList.Length)
        {
            for (int i = 0; i < _ImageSlotList.Length; i++)
            {
                if (i == slot)
                {
                    _ImageSlotList[i].sprite = _selectedSlot;
                }
                else
                {
                    _ImageSlotList[i].sprite = _normalSlot;
                }
            }
        }
    }

    public void InsertItemIcon(int slot, Sprite icon)
    {
        if (slot >= 0 && slot < _ImageIconList.Length)
        {
            _ImageIconList[slot].sprite = icon;
            _ImageIconList[slot].enabled = true;
        }
    }

    public void RemoveItemIcon(int slot)
    {
        if (slot >= 0 && slot < _ImageIconList.Length)
        {
            _ImageIconList[slot].sprite = null;
            _ImageIconList[slot].enabled = false;
        }
    }

    public void ShowPauseMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _HUDUIContainer.SetActive(false);
        _PauseMenuContainer.SetActive(true);
    }

    public void HidePauseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _PauseMenuContainer.SetActive(false);
        _HUDUIContainer.SetActive(true);
    }

    public void GoToSettingsMenu()
    {
        _PauseMenuElements.SetActive(false);
        _OptionsMenuContainer.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        _OptionsMenuContainer.SetActive(false);
        _PauseMenuElements.SetActive(true);
        
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
