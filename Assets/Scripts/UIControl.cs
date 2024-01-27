using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private PlayerStats playerStats;
    [SerializeField] private BattleStats battleStats;

    [Header("Widgets")] [SerializeField] private TMP_Text currentScore;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private Slider laughBar;
    [SerializeField] private TMP_Text remainingTime;

    private void Start()
    {
        laughBar.maxValue = playerStats.maximumLaugh;
    }

    private void Update()
    {
        currentScore.text = Mathf.RoundToInt(playerStats.currentScore).ToString();
        laughBar.value = playerStats.currentLaugh;
        bestScore.text = Mathf.RoundToInt(GameStats.bestScore).ToString();
        remainingTime.text = Mathf.RoundToInt(battleStats.remainingTime).ToString();
    }

    public void ShowWelcome()
    {
    }
}