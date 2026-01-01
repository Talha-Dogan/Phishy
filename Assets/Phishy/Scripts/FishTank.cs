using UnityEngine;

public class FishTank : MonoBehaviour
{

    [Header("Background Settings")]
    public SpriteRenderer backgroundRenderer;
    public Sprite[] aquariumSprites;

    public void UpdateTankVisuals(int score)
    {

        if (backgroundRenderer != null && aquariumSprites.Length > 0)
        {

            int spriteIndex = Mathf.Clamp(score, 0, aquariumSprites.Length - 1);

            backgroundRenderer.sprite = aquariumSprites[spriteIndex];
        }
    }

    public void PlayFeedbackAnimation(bool isCorrect)
    {
    }
}