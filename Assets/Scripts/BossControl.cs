using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossStats))]
public class BossControl : MonoBehaviour
{
    [SerializeField] private HitBox tickler;

    private CharacterController _controller;
    private Animator _animator;
    private BossStats _stats;
    private static readonly int AnimParamTickle = Animator.StringToHash("Tickle");

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stats = GetComponent<BossStats>();
        tickler.OnHitPlayer += OnTickleHit;
    }

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        _controller.SimpleMove(_stats.speed * direction.normalized);
    }

    public bool NoAction => !Tickling;

    public bool Tickling { get; private set; }
    private bool _listeningTickleHit;

    public void BeginTickle()
    {
        _animator.SetTrigger(AnimParamTickle);
        Tickling = true;
        _listeningTickleHit = true;
    }

    public void OnTickle()
    {
        // Physics.Raycast(transform.position, transform.forward, _stats.attackRange);
    }

    public void OnTickleHit(object sender, PlayerControl playerControl)
    {
        // avoid: 1) multi-hit during a single tickle 2) hit when not tickling
        if (!_listeningTickleHit) return;

        playerControl.TakeTickleHit(_stats.tickleDamage);
        _listeningTickleHit = false;
    }

    public void OnTickleFinish()
    {
        Tickling = false;
        _listeningTickleHit = false;
    }
}