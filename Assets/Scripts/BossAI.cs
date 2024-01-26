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

    private void Start()
    {
        _control = GetComponent<BossControl>();
        _stats = GetComponent<BossStats>();
    }

    private void Update()
    {
        if (!_control.NoAction) return;

        Vector3 direction = opponent.position - transform.position;
        bool withinAttackRange = direction.sqrMagnitude <= _stats.attackRange * _stats.attackRange;
        if (withinAttackRange)
        {
            _control.BeginTickle();
        }
        else
        {
            direction.Normalize();
            _control.Move(direction);
        }
    }
}