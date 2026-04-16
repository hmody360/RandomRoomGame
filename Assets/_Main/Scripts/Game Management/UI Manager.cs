using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _promptText;
    [SerializeField] private GameObject _promptContainer;

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

    public void StartPromptCoroutine(float time, string text) //Start the Text Prompt Coroutine with a set time and text
    {
        StartCoroutine(ShowPromptText(time, text));
    }

    private IEnumerator ShowPromptText(float time, string text) // The Text Prompt Coroutine with a set time and text
    {
        ShowPrompt(text);
        yield return new WaitForSeconds(time);
        HidePrompt();
    }
}
