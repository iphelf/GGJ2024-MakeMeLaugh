using UnityEngine;

[RequireComponent(typeof(BossControl))]
[RequireComponent(typeof(BossStats))]
public class BossAI : MonoBehaviour
{
    private Transform _opponent;
    [SerializeField] private AnimationCurve approachWeight;
    [SerializeField] private AnimationCurve tickleWeight;
    public float chaseProbability = 0.5f;
    [SerializeField] private AnimationCurve jokeWeight;
    [SerializeField] private AnimationCurve actWeight;

    private BossControl _control;
    private BossStats _stats;
    private Stage _stage;

    private readonly Stage[] _allStages =
    {
        new TickleOnlyStage(),
        new JokeOnlyStage(),
        new ActOnlyStage(),
    };

    public enum StageType
    {
        TickerOnly = 0,
        JokeOnly = 1,
        ActOnly = 2,
        All = 3,
    }

    public StageType startingStage;

    private void Start()
    {
        _control = GetComponent<BossControl>();
        _stats = GetComponent<BossStats>();
        _opponent = _stats.playerTransform;
        _stage = _allStages[(int)startingStage];
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

            Vector3 direction = self._opponent.position - self.transform.position;
            bool withinAttackRange = direction.sqrMagnitude <= self._stats.tickleRange * self._stats.tickleRange;
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
            self._control.LookAt(self._opponent.position);

            if (!self._control.NoAction) return;

            self._control.BeginJoke();
        }
    }

    private class ActOnlyStage : Stage
    {
        public override void Update(BossAI self)
        {
            self._control.LookAtStamp(self._opponent.position);

            if (!self._control.NoAction) return;

            self._control.BeginAct();
        }
    }

    private void Update()
    {
        _stage?.Update(this);
    }
}