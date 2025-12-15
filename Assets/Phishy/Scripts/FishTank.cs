using UnityEngine;

public class FishTank : MonoBehaviour
{
    public Animator tankAnimator; // Inspector'dan atanacak (Animator: Fishtank)

    // Belirli bir animasyonu oynat
    public void PlayAnimation(string animName)
    {
        if (tankAnimator != null)
            tankAnimator.Play(animName);
        else
            Debug.LogWarning("FishTank: Animator atanmadı!");
    }

    // Skora göre animasyonu oynat
    public void PlayScoreAnimation(int score)
    {
        if (tankAnimator == null)
        {
            Debug.LogWarning("FishTank: Animator bulunamadı!");
            return;
        }

        // Score'u 1-6 arası sınırlıyoruz
        int animIndex = Mathf.Clamp(score, 1, 6);
        string animName = animIndex + "Fish";

        tankAnimator.Play(animName);
    }

    // Rastgele animasyon oynat (opsiyonel)
    public void PlayRandomAnimation()
    {
        if (tankAnimator == null)
        {
            Debug.LogWarning("FishTank: Animator bulunamadı!");
            return;
        }

        string[] animations = { "1Fish", "2Fish", "6Fish" };
        int index = Random.Range(0, animations.Length);
        tankAnimator.Play(animations[index]);
    }
}
