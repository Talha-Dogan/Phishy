using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    // ... (Diğer değişkenlerin aynen kalıyor) ...
    [Header("Player Reference")]
    public FishScoreSystem playerScript;

    [Header("UI References")]
    public TMP_Text question;
    public TMP_Text[] buttonText;
    public Button nextButton;
    public Image[] buttonImages;

    [Header("Colors")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.white;

    private List<quizQuestions> activeQuestions = new List<quizQuestions>();

    int currentQuestion = 0;
    int score = 0;
    bool hasAnswered = false;

    public FishTank fishSystem;

    // ... (Start, LoadQuestions, RestartGame, updateQuestion fonksiyonların AYNI kalıyor) ...
    void Start()
    {
        if (fishSystem == null)
            fishSystem = FindObjectOfType<FishTank>();

        nextButton.interactable = false;

        string selectedLang = PlayerPrefs.GetString("SelectedLanguage", "EN");
        string fileName = "QuestionsData_" + selectedLang;
        LoadQuestions(fileName);
    }

    void LoadQuestions(string fileName)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            QuestionWrapper data = JsonUtility.FromJson<QuestionWrapper>(jsonFile.text);

            if (data != null && data.questions != null)
            {
                activeQuestions = data.questions;
                RestartGame();
            }
        }
    }

    void RestartGame()
    {
        currentQuestion = 0;
        score = 0;
        // Oyuna başlarken kayıtlı skoru sıfırlıyoruz ki eski oyunun puanı kalmasın
        PlayerPrefs.SetInt("CurrentScore", 0);

        buttonText[4].text = "0 Fish";
        if (playerScript != null) playerScript.ResetCharacter();
        if (fishSystem != null) fishSystem.UpdateTankVisuals(0);
        updateQuestion();
    }

    void updateQuestion()
    {
        if (activeQuestions == null || activeQuestions.Count == 0) return;
        if (currentQuestion >= activeQuestions.Count) return;

        if (playerScript != null) playerScript.ResetCharacter();
        hasAnswered = false;

        question.text = activeQuestions[currentQuestion].Q;

        buttonText[0].text = activeQuestions[currentQuestion].A;
        buttonText[1].text = activeQuestions[currentQuestion].B;
        buttonText[2].text = activeQuestions[currentQuestion].C;
        buttonText[3].text = activeQuestions[currentQuestion].D;

        foreach (Image img in buttonImages) if (img != null) img.color = defaultColor;
    }

    // --- BURASI DÜZENLENDİ ---
    public void answerClick(string answer)
    {
        if (hasAnswered) return;
        if (activeQuestions == null || activeQuestions.Count == 0) return;
        if (currentQuestion >= activeQuestions.Count) return;

        hasAnswered = true;

        string correctAnswerString = activeQuestions[currentQuestion].answer;
        char correctAnswerChar = correctAnswerString[0];

        int clickedIndex = answer[0] - 'A';
        int correctIndex = correctAnswerChar - 'A';

        clickedIndex = Mathf.Clamp(clickedIndex, 0, 3);
        correctIndex = Mathf.Clamp(correctIndex, 0, 3);

        if (answer[0] == correctAnswerChar)
        {
            score++;
            buttonText[4].text = score.ToString() + " Fish";

            // YENİ EKLENEN KISIM: Puan artınca kaydet
            PlayerPrefs.SetInt("CurrentScore", score);

            if (buttonImages.Length > clickedIndex) buttonImages[clickedIndex].color = correctColor;
            if (fishSystem != null) { fishSystem.UpdateTankVisuals(score); fishSystem.PlayFeedbackAnimation(true); }
            if (playerScript != null) playerScript.PlayCatchAnimation(true);
        }
        else
        {
            if (score > 0) score--;
            buttonText[4].text = score.ToString() + " Fish";

            // YENİ EKLENEN KISIM: Puan azalınca kaydet
            PlayerPrefs.SetInt("CurrentScore", score);

            if (buttonImages.Length > clickedIndex) buttonImages[clickedIndex].color = wrongColor;
            if (buttonImages.Length > correctIndex) buttonImages[correctIndex].color = correctColor;
            if (fishSystem != null) { fishSystem.UpdateTankVisuals(score); fishSystem.PlayFeedbackAnimation(false); }
            if (playerScript != null) playerScript.PlayCatchAnimation(false);
        }

        // Değişikliği diske hemen yaz (Garanti olsun)
        PlayerPrefs.Save();

        currentQuestion++;
        nextButton.interactable = true;
    }

    public void nextClick()
    {
        updateQuestion();
        nextButton.interactable = false;
    }
}

// ... (Class tanımları aynı kalıyor) ...
[System.Serializable]
public class quizQuestions
{
    public string Q;
    public string A;
    public string B;
    public string C;
    public string D;
    public string answer;
}

[System.Serializable]
public class QuestionWrapper
{
    public List<quizQuestions> questions;
}