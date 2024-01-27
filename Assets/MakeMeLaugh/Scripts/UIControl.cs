using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MakeMeLaugh.Scripts
{
    public class UIControl : MonoBehaviour
    {
        [Header("Stats")] [SerializeField] private PlayerStats playerStats;
        [SerializeField] private BattleStats battleStats;

        [Header("Widgets")] [SerializeField] private TMP_Text currentScore;
        [SerializeField] private TMP_Text bestScore;
        [SerializeField] private Slider laughBar;
        [SerializeField] private TMP_Text remainingTime;
        [SerializeField] private Image lightAdaption;

        private void Start()
        {
            laughBar.maxValue = playerStats.maximumLaugh;
        }

        private void Update()
        {
            currentScore.text = Mathf.RoundToInt(playerStats.currentScore).ToString();
            laughBar.value = playerStats.currentLaugh;
            bestScore.text = GameStats.bestScore.ToString();
            remainingTime.text = Mathf.RoundToInt(battleStats.remainingTime).ToString();

            UpdateLightAdaption();
        }

        private float _darknessTime;
        private float _adaptingDuration;

        private void UpdateLightAdaption()
        {
            var newColor = lightAdaption.color;

            if (PlayerCondition.eyesClosed)
            {
                _darknessTime += Time.deltaTime;
                _adaptingDuration = _darknessTime;
                newColor.a = 0.0f;
            }
            else if (_darknessTime > 0.0f)
            {
                _darknessTime = Mathf.Max(0.0f,
                    _darknessTime - Time.deltaTime * playerStats.lightAdaptability);
                newColor.a = _darknessTime / _adaptingDuration;
            }

            lightAdaption.color = newColor;
        }
    }
}