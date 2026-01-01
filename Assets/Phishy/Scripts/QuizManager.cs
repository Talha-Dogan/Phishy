using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Player Reference")]
    public FishScoreSystem playerScript;

    public TMP_Text question;
    public TMP_Text[] buttonText;
    public Button nextButton;

    public List<quizQuestions> questions = new List<quizQuestions>();
    int currentQuestion = 0;
    int score = 0;

    bool hasAnswered = false;

    public FishTank fishSystem;

    void Start()
    {
        updateQuestion();
        nextButton.interactable = false;

        if (fishSystem == null)
            fishSystem = FindObjectOfType<FishTank>();
    }

    void updateQuestion()
    {
        if (currentQuestion >= questions.Count) return;

        if (playerScript != null)
        {
            playerScript.ResetCharacter();
        }

        hasAnswered = false;

        question.text = questions[currentQuestion].Q;

        buttonText[0].text = questions[currentQuestion].A;
        buttonText[1].text = questions[currentQuestion].B;
        buttonText[2].text = questions[currentQuestion].C;
        buttonText[3].text = questions[currentQuestion].D;
    }

    public void answerClick(string answer)
    {
        if (hasAnswered) return;

        if (currentQuestion >= questions.Count) return;

        hasAnswered = true;

        if (answer[0] == questions[currentQuestion].answer)
        {
            score += 1;
            buttonText[4].text = score.ToString() + " fish";

            if (fishSystem != null)
            {
                fishSystem.UpdateTankVisuals(score);
                fishSystem.PlayFeedbackAnimation(true);
            }

            if (playerScript != null)
            {
                playerScript.PlayCatchAnimation(true);
            }
        }
        else
        {
            if (score > 0)
            {
                score -= 1;
            }

            buttonText[4].text = score.ToString() + " fish";

            if (fishSystem != null)
            {
                fishSystem.UpdateTankVisuals(score);
                fishSystem.PlayFeedbackAnimation(false);
            }

            if (playerScript != null)
            {
                playerScript.PlayCatchAnimation(false);
            }
        }

        currentQuestion++;
        nextButton.interactable = true;
    }

    public void nextClick()
    {
        updateQuestion();
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