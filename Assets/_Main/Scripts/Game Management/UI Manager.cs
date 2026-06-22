using System.Collections;
using TMPro;
using UnityEngine;
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
}
