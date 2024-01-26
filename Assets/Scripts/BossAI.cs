using UnityEngine;

[RequireComponent(typeof(BossControl))]
[RequireComponent(typeof(BossStats))]
public class BossAI : MonoBehaviour
{
    private Transform _opponent;
    [SerializeField] private AnimationCurve approachWeight = AnimationCurve.Linear(0.0f, 200.0f, 20.0f, 50.0f);
    public float chaseProbability = 0.5f;
    [SerializeField] private AnimationCurve jokeWeight = AnimationCurve.Constant(0.0f, 20.0f, 50.0f);
    [SerializeField] private AnimationCurve actWeight = AnimationCurve.Constant(0.0f, 20.0f, 50.0f);

    private BossControl _control;
    private BossStats _stats;
    private Stage _stage;

    private readonly Stage[] _allStages =
    {
        new TickleOnlyStage(),
        new JokeOnlyStage(),
        new ActOnlyStage(),
        new AllInOneStage(),
    };

    private enum ActionType
    {
        Ticker = 0,
        Joke = 1,
        Act = 2,
        Approach = 3,
    }

    public enum StageType
    {
        TickerOnly = ActionType.Ticker,
        JokeOnly = ActionType.Joke,
        ActOnly = ActionType.Act,
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

    private ActionType GenerateNextTask()
    {
        float distance = (_opponent.position - transform.position).magnitude;
        float approach = approachWeight.Evaluate(distance);
        float joke = PlayerCondition.earsHeld ? 0.0f : jokeWeight.Evaluate(distance);
        float act = PlayerCondition.eyesClosed ? 0.0f : actWeight.Evaluate(distance);
        float dice = Random.Range(0.0f, approach + joke + act);
        if (dice <= approach) return ActionType.Approach;
        if (dice <= approach + joke) return ActionType.Joke;
        return ActionType.Act;
    }

    private class AllInOneStage : Stage
    {
        private bool _isChasing;
        private float _remainingChasingTime;

        public override void Update(BossAI self)
        {
            var opponentPosition = self._opponent.position;
            self._control.LookAt(opponentPosition);
            Vector3 direction = opponentPosition - self.transform.position;
            bool withinAttackRange = direction.sqrMagnitude <= self._stats.tickleRange * self._stats.tickleRange;

            if (_isChasing)
            {
                if (withinAttackRange)
                {
                    OnChaseFinish();
                    self._control.BeginTickle();
                    return;
                }

                direction.Normalize();
                self._control.Move(direction);
                _remainingChasingTime -= Time.deltaTime;
                if (_remainingChasingTime <= 0.0f)
                    OnChaseFinish();
            }

            if (!self._control.NoAction) return;

            ActionType task = self.GenerateNextTask();
            switch (task)
            {
                case ActionType.Joke:
                    self._control.BeginJoke();
                    break;
                case ActionType.Act:
                    self._control.BeginAct();
                    break;
                default:
                    BeginChase(self);
                    break;
            }
        }

        private void BeginChase(BossAI self)
        {
            _isChasing = true;
            _remainingChasingTime = Random.Range(self._stats.chasingDurationMin, self._stats.chasingDurationMax);
        }

        private void OnChaseFinish()
        {
            _isChasing = false;
        }
    }

    private void Update()
    {
        _stage?.Update(this);
    }
}