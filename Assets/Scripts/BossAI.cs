using UnityEngine;

[RequireComponent(typeof(BossControl))]
[RequireComponent(typeof(BossStats))]
public class BossAI : MonoBehaviour
{
    [SerializeField] private Transform opponent;
    [SerializeField] private AnimationCurve approachWeight;
    [SerializeField] private AnimationCurve tickleWeight;
    public float chaseProbability = 0.5f;
    [SerializeField] private AnimationCurve jokeWeight;
    [SerializeField] private AnimationCurve actWeight;

    private BossControl _control;
    private BossStats _stats;
    private Stage _stage;

    private void Start()
    {
        _control = GetComponent<BossControl>();
        _stats = GetComponent<BossStats>();
        _stage = new TickleOnlyStage();
        // _stage = new JokeOnlyStage();
    }

    private abstract class Stage
    {
        public abstract void Update(BossAI self);
    }

    private class TickleOnlyStage : Stage
    {
        public override void Update(BossAI self)
        {
            if (!self._control.NoAction) return;

            Vector3 direction = self.opponent.position - self.transform.position;
            bool withinAttackRange = direction.sqrMagnitude <= self._stats.attackRange * self._stats.attackRange;
            if (withinAttackRange)
            {
                self._control.BeginTickle();
            }
            else
            {
                direction.Normalize();
                self._control.Move(direction);
            }
        }
    }

    private class JokeOnlyStage : Stage
    {
        public override void Update(BossAI self)
        {
        }
    }

    private void Update()
    {
        _stage?.Update(this);
    }
}