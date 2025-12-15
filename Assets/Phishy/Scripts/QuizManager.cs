using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class quizStuff : MonoBehaviour
{
    public TMP_Text question;
    public TMP_Text[] buttonText;
    public Button nextButton;

    public List<quizQuestions> questions = new List<quizQuestions>();
    int currentQuestion = 0;    // pointer to question
    int score = 0;

    // Balık animasyon sistemi
    public FishScoreSystem fishSystem;

    void Start()
    {
        updateQuestion();
        nextButton.interactable = false;

        // Eğer inspector'dan atanmadıysa sahnede bul
        if (fishSystem == null)
            fishSystem = FindObjectOfType<FishScoreSystem>();
    }

    void updateQuestion()
    {
        question.text = questions[currentQuestion].Q;

        buttonText[0].text = questions[currentQuestion].A;
        buttonText[1].text = questions[currentQuestion].B;
        buttonText[2].text = questions[currentQuestion].C;
        buttonText[3].text = questions[currentQuestion].D;
    }

    public void answerClick(string answer)
    {
        // No validation to prevent out-of-turn clicking!!!

        if (answer[0] == questions[currentQuestion].answer)
        {
            score += 1;
            buttonText[4].text = "+" + score.ToString() + " fish";

            // ✅ Doğruysa +Fish animasyonu
            if (fishSystem != null)
                fishSystem.fishAnimator.Play("+Fish");
        }
        else
        {
            buttonText[4].text = "-" + score.ToString() + " fish";

            // ❌ Yanlışsa -Fish animasyonu
            if (fishSystem != null)
                fishSystem.fishAnimator.Play("-Fish");
        }

        currentQuestion++; // go to next question
        nextButton.interactable = true; // set next button clickable
    }

    public void nextClick()
    {
        updateQuestion(); // move to next question
        nextButton.interactable = false;
    }
}

[System.Serializable]
public class quizQuestions
{
    public string Q;
    public string A;
    public string B;
    public string C;
    public string D;
    public char answer;
}
