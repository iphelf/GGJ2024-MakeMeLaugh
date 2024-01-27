public static class GameStats
{
    public class BattleResult
    {
        public enum LaughType
        {
            None,
            Normal,
            Lol,
            Lofl,
        }

        public LaughType laughPeak;
        public float laughingTime;
        public int laughCount;
        public float score;
    }

    public static int bestScore;
    public static int worstScore;
    public static BattleResult lastResult;
}