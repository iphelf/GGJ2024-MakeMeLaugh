using UnityEngine;

[RequireComponent(typeof(BossControl))]
[RequireComponent(typeof(BossStats))]
public class BossAI : MonoBehaviour
{
    private Transform _opponent;
    [SerializeField] private AnimationCurve approachWeight = AnimationCurve.Linear(0.0f, 200.0f, 20.0f, 50.0f);
    [SerializeField] private AnimationCurve jokeWeight = AnimationCurve.Constant(0.0f, 20.0f, 50.0f);
    [SerializeField] private AnimationCurve actWeight = AnimationCurve.Constant(0.0f, 20.0f, 50.0f);

    private BossControl _control;
    private BossStats _stats;
    private Stage _stage;

    private readonly Stage[] _allStages =
    {
        new NoopStage(),
        new TickleOnlyStage(),
        new JokeOnlyStage(),
        new ActOnlyStage(),
        new AllInOneStage(),
    };

    private enum ActionType
    {
        None = 0,
        Ticker = 1,
        Joke = 2,
        Act = 3,
        Approach = 4,
    }

    public enum StageType
    {
        None = 0,
        TickerOnly = ActionType.Ticker,
        JokeOnly = ActionType.Joke,
        ActOnly = ActionType.Act,
        All = 4,
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

    private class NoopStage : Stage
    {
        public override void Update(BossAI self)
        {
        }
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
                self.OnChaseFinish();
                self._control.BeginTickle();
            }
            else if (!self._isChasing)
            {
                self.BeginChase();
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
        private float _remainingDecisionTime;
        private float _remainingChasingTime;

        public override void Update(BossAI self)
        {
            var opponentPosition = self._opponent.position;
            self._control.LookAt(opponentPosition);
            Vector3 direction = opponentPosition - self.transform.position;
            bool withinAttackRange = direction.sqrMagnitude <= self._stats.tickleRange * self._stats.tickleRange;

            if (self._isChasing)
            {
                _remainingChasingTime -= Time.deltaTime;
                if (withinAttackRange || _remainingChasingTime <= 0.0f)
                    self.OnChaseFinish();
            }

            if (withinAttackRange)
            {
                if (!self._control.Tickling)
                    self._control.BeginTickle();
            }
            else
            {
                _remainingDecisionTime -= Time.deltaTime;
                if (_remainingDecisionTime <= 0.0f)
                {
                    if (!PlayerCondition.earsHeld && !self._control.Joking)
                        self._control.BeginJoke();
                    if (!PlayerCondition.eyesClosed && !self._control.Acting)
                        self._control.BeginAct();
                    if (!self._isChasing && self.GenerateNextTask() == ActionType.Approach)
                        BeginChase(self);
                    _remainingDecisionTime = self._stats.decisionCoolDown;
                }
            }
        }

        private void BeginChase(BossAI self)
        {
            self.BeginChase();
            _remainingChasingTime = Random.Range(self._stats.chasingDurationMin, self._stats.chasingDurationMax);
        }
    }

    private bool _isChasing;

    private void BeginChase()
    {
        _isChasing = true;
        _control.BeginChase();
    }

    private void OnChaseFinish()
    {
        _isChasing = false;
        _control.EndChase();
    }

    private void Update()
    {
        if (_isChasing)
            _control.Move(_opponent.position - transform.position);

        _stage?.Update(this);
    }
}