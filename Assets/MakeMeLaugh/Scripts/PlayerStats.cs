using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    public class PlayerStats : MonoBehaviour
    {
        public float maximumLaugh = 300.0f;
        public float currentLaugh = 0.0f;
        public float laughAttenuation = 20.0f;
        public Sprite notLaughingSprite;
        public float normalLaughThreshold = 1.0f / 3.0f;
        public Sprite normalLaughSprite;
        public float lolThreshold = 1.9f / 3.0f;
        public Sprite lolSprite;
        public float loflThreshold = 2.8f / 3.0f;
        public Sprite loflSprite;
        public AnimationCurve gainRate = AnimationCurve.EaseInOut(0.0f, 10.0f, 300.0f, -30.0f);
        public float currentScore = 0.0f;
        public float speed = 5.0f;
        public float lightAdaptability = 3.0f;
    }
}