using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maximumLaugh = 300.0f;
    public float currentLaugh = 0.0f;
    public float laughAttenuation = 20.0f;
    public AnimationCurve gainRate = AnimationCurve.EaseInOut(0.0f, 10.0f, 300.0f, -30.0f);
    public float currentScore = 0.0f;
    public float speed = 5.0f;
}