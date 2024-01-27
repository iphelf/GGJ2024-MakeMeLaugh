using UnityEngine;

[RequireComponent(typeof(GameStats))]
public class GameControl : MonoBehaviour
{
    private GameStats _stats;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        _stats = GetComponent<GameStats>();
    }

    private void Update()
    {
        _stats.currentScore += _stats.gainRate.Evaluate(playerStats.currentLaugh) * Time.deltaTime;
    }
}