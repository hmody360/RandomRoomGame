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

    public void ShowPrompt(string text)
    {
        _promptText.text = text;
        _promptContainer.SetActive(true);
    }

    public void HidePrompt()
    {
        _promptText.text = "";
        _promptContainer.SetActive(false);
    }

    public void StartPromptCoroutine(float time, string text)
    {
        StartCoroutine(ShowPromptText(time, text));
    }

    private IEnumerator ShowPromptText(float time, string text)
    {
        ShowPrompt(text);
        yield return new WaitForSeconds(time);
        HidePrompt();
    }
}
