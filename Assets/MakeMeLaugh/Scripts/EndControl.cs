﻿using TMPro;
using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    public class EndControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text subtitle;

        private void Start()
        {
            GameStats.BattleResult result = GameStats.lastResult;
            // GameStats.BattleResult result = new()
            // {
            //     laughCount = 1,
            //     laughingTime = 30.12345f,
            //     score = 12300.12345f
            // };

            title.text = ComposeTitle(result);
            subtitle.text = ComposeSubtitle(result);

            GameStats.bestScore = Mathf.Max(GameStats.bestScore, Mathf.RoundToInt(result.score));
            GameStats.worstScore = Mathf.Min(GameStats.worstScore, Mathf.RoundToInt(result.score));
        }

        private string ComposeSubtitle(GameStats.BattleResult result)
        {
            int score = Mathf.RoundToInt(result.score);
            string scoreDesc = $"And your score is {score}";
            if (score > GameStats.bestScore)
                return scoreDesc + ", best ever!";
            if (score < GameStats.worstScore)
                return scoreDesc + ", lowest ever!";
            return scoreDesc + ".";
        }

        private string ComposeTitle(GameStats.BattleResult result)
        {
            if (result.laughCount == 0)
                return "You didn't laugh.\n** Sigh **\nWell done.";
            string main =
                result.laughCount == 1
                    ? $"You laughed once for {result.laughingTime:0.0} seconds."
                    : $"You laughed {result.laughCount} times for {result.laughingTime:0.0} seconds.";
            return result.laughPeak switch
            {
                GameStats.BattleResult.LaughType.Lol => $"LOL\n{main}",
                GameStats.BattleResult.LaughType.Lofl => $"LOFL\n{main}",
                _ => main,
            };
        }

        public void ReturnToTitle()
        {
            GameControl.OpenEntry();
        }

        public void AnotherRound()
        {
            GameControl.OpenBattle();
        }
    }
}