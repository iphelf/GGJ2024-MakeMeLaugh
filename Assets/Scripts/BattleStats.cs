using UnityEngine;

public class BattleStats : MonoBehaviour
{
    public class BattleResult
    {
        public float laughPeak;
        public float laughingTime;
        public float laughCount;
        public float score;
    }

    public float duration = 60.0f;
    [HideInInspector] public float remainingTime;
}