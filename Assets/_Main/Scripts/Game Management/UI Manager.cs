using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Prompt UI Elements")]
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject _promptContainer;

    [Header("Interact UI Elements")]
    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private GameObject _interactContainer;

    [Header("Inventory UI Elements")]
    [SerializeField] private Sprite _normalSlot;
    [SerializeField] private Sprite _selectedSlot;
    [SerializeField] private Image[] _ImageSlotList;
    [SerializeField] private Image[] _ImageIconList;

    private Coroutine PromptTextCoroutine;

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
    private void ShowPrompt(string text) //Set Text and Show it
    {
        _promptText.text = text;
        _promptContainer.SetActive(true);
    }

    private void HidePrompt() //Set Text To Blank and Hide it
    {
        _promptText.text = "";
        _promptContainer.SetActive(false);
    }

    public void StartPromptCoroutine(float time, string text) //Start the Text Prompt Coroutine with a set time and text
    {
        if (PromptTextCoroutine == null)
        {
            PromptTextCoroutine = StartCoroutine(ShowPromptText(time, text));
        }

    }

    private IEnumerator ShowPromptText(float time, string text) // The Text Prompt Coroutine with a set time and text
    {
        ShowPrompt(text);
        yield return new WaitForSeconds(time);
        HidePrompt();
        PromptTextCoroutine = null;
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
