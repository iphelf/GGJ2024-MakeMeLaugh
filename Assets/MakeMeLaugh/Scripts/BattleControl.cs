using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(BattleStats))]
    public class BattleControl : MonoBehaviour
    {
        private BattleStats _stats;

        private void Start()
        {
            _stats = GetComponent<BattleStats>();
            _stats.remainingTime = _stats.duration;
            GameStats.lastResult = new GameStats.BattleResult();
        }

        private void Update()
        {
            _stats.remainingTime -= Time.deltaTime;
            if (_stats.remainingTime <= 0.0f)
                OnBattleEnd();
        }

        private void OnBattleEnd()
        {
            GameControl.OpenEnd();
        }
    }
}