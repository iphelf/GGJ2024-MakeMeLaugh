using UnityEngine;

[RequireComponent(typeof(BossControl))]
public class BossAI : MonoBehaviour
{
    [SerializeField] private Transform opponent;
    [SerializeField] private AnimationCurve approachWeight;
    [SerializeField] private AnimationCurve tickleWeight;
    public float chaseProbability = 0.5f;
    [SerializeField] private AnimationCurve jokeWeight;
    [SerializeField] private AnimationCurve actWeight;

    private BossControl _control;
    public float attackRange = 1.5f;

    private void Start()
    {
        _control = GetComponent<BossControl>();
    }

    private void Update()
    {
        if (!_control.NoAction) return;

        Vector3 direction = opponent.position - transform.position;
        bool withinAttackRange = direction.sqrMagnitude <= attackRange * attackRange;
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