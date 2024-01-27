using UnityEngine;

public class GameStats : MonoBehaviour
{
    public AnimationCurve gainRate = AnimationCurve.EaseInOut(0.0f, 10.0f, 300.0f, -30.0f);
    public float currentScore;
    public float bestScore;
}