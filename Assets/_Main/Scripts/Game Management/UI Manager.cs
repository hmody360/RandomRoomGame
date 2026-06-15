using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject _promptContainer;

    [SerializeField] private TextMeshProUGUI _interactText;
    [SerializeField] private GameObject _interactContainer;

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

    public void ShowPrompt(string text) //Set Text and Show it
    {
        _promptText.text = text;
        _promptContainer.SetActive(true);
    }

    public void HidePrompt() //Set Text To Blank and Hide it
    {
        _promptText.text = "";
        _promptContainer.SetActive(false);
    }

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

    public void StartPromptCoroutine(float time, string text) //Start the Text Prompt Coroutine with a set time and text
    {
        if(PromptTextCoroutine == null)
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
}
