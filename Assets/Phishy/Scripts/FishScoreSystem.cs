using UnityEngine;

public class FishScoreSystem : MonoBehaviour
{
    public Animator fishAnimator;

    public void PlayCatchAnimation(bool isCorrect)
    {
        if (fishAnimator != null)
        {
            if (isCorrect)
            {
                fishAnimator.ResetTrigger("FishMiss");
                fishAnimator.SetTrigger("FishHit");
            }
            else
            {
                fishAnimator.ResetTrigger("FishHit");
                fishAnimator.SetTrigger("FishMiss");
            }
        }
        else
        {
            Debug.LogError("Player Animator Missing");
        }
    }

    public void ResetCharacter()
    {
        if (fishAnimator != null)
        {
            fishAnimator.ResetTrigger("FishHit");
            fishAnimator.ResetTrigger("FishMiss");
            fishAnimator.Rebind();
            fishAnimator.Update(0f);
        }
    }
}