using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "GameScene";

    [Header("UI References")]
    public Button playButton;
    public Image enButtonImage;
    public Image trButtonImage;

    [Header("Colors")]
    public Color selectedColor = Color.green;
    public Color defaultColor = Color.white;

    private string currentSelectedLang = "";

    void Start()
    {
        if (playButton != null)
            playButton.interactable = false;

        ResetButtonColors();
    }

    public void SelectLanguage(string langCode)
    {
        currentSelectedLang = langCode;

        if (playButton != null) playButton.interactable = true;
        UpdateButtonVisuals(langCode);
    }

    public void PlayGame()
    {
        if (currentSelectedLang == "") return;

        PlayerPrefs.SetString("SelectedLanguage", currentSelectedLang);
        PlayerPrefs.Save();

        SceneManager.LoadScene(gameSceneName);
    }

    public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    void UpdateButtonVisuals(string lang)
    {
        ResetButtonColors();

        if (lang == "EN" && enButtonImage != null) enButtonImage.color = selectedColor;
        else if (lang == "TR" && trButtonImage != null) trButtonImage.color = selectedColor;
    }

    void ResetButtonColors()
    {
        if (enButtonImage != null) enButtonImage.color = defaultColor;
        if (trButtonImage != null) trButtonImage.color = defaultColor;
    }
}