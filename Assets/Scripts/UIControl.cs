using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private GameStats gameStats;
    [SerializeField] private PlayerStats playerStats;

    [Header("Widgets")] [SerializeField] private TMP_Text currentScore;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private Slider laughBar;

    private void Start()
    {
        laughBar.maxValue = playerStats.maximumLaugh;
    }

    private void Update()
    {
        currentScore.text = Mathf.RoundToInt(gameStats.currentScore).ToString();
        bestScore.text = Mathf.RoundToInt(gameStats.bestScore).ToString();
        laughBar.value = playerStats.currentLaugh;
    }
}