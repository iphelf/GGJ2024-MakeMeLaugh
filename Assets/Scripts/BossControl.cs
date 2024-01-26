using System.Collections;
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
    private Transform _cameraTransform;
    private PlayerControl _player;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stats = GetComponent<BossStats>();
        _cameraTransform = Camera.main!.transform;
        _player = _stats.playerTransform.GetComponent<PlayerControl>();

        tickler.OnHitPlayer += OnTickleHit;
    }

    public void LookAt(Vector3 position)
    {
        transform.rotation = Quaternion.LookRotation(position - transform.position);
    }

    public void LookAtStamp(Vector3 position)
    {
        transform.rotation = Quaternion.LookRotation(position - transform.position);
    }

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        _controller.SimpleMove(_stats.speed * direction.normalized);
    }

    public bool NoAction => !Tickling && !Joking && !Acting;

    #region Tickle

    private static readonly int AnimParamTickle = Animator.StringToHash("Tickle");
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

    #endregion

    #region Joke

    private static readonly int AnimParamJoke = Animator.StringToHash("Joke");

    public bool Joking { get; private set; }

    public void BeginJoke()
    {
        _animator.SetBool(AnimParamJoke, true);
        Joking = true;
        StartCoroutine(Joke());
    }

    private IEnumerator Joke()
    {
        yield return new WaitForSeconds(_stats.jokePreparationTime);

        int index = Random.Range(0, _stats.jokes.jokes.Count);
        string joke = _stats.jokes.jokes[index];
        string[] tokens = joke.Split(" ");
        foreach (var token in tokens)
        {
            var bossTransform = transform;
            var go = Instantiate(_stats.jokeTokenPrefab, bossTransform.position, bossTransform.rotation);
            yield return null;
            if (go == null) continue;
            var wordBullet = go.GetComponent<WordBullet>();
            wordBullet.SetWord(token, out var width);
            wordBullet.SetDamage(_stats.jokeDamage / tokens.Length);
            go.transform.Translate(0.0f, 0.0f, -width / 2.0f);
            if (Vector3.Dot(go.transform.forward, _cameraTransform.right) > 0.0f)
                // so that text is always in a readable orientation
                wordBullet.FlipUpsideDown();
            wordBullet.Shoot(_stats.jokeSpeed);
            yield return new WaitForSeconds((width + _stats.jokeTokenSpace) / _stats.jokeSpeed);
        }

        yield return new WaitForSeconds(_stats.jokeRecoveryTime);
        OnJokeFinish();
    }

    private void OnJokeFinish()
    {
        Joking = false;
    }

    #endregion

    #region Act

    private static readonly int AnimParamAct = Animator.StringToHash("Act");

    public bool Acting { get; private set; }
    private float _actDuration;

    /// minDuration = -1, maxDuration = -1 => duration = random(stats)
    /// minDuration > 0, maxDuration = -1 => duration = minDuration
    /// minDuration > 0, maxDuration > 0 => duration = random(min, max)
    public void BeginAct(float minDuration = -1.0f, float maxDuration = -1.0f)
    {
        _animator.SetBool(AnimParamAct, true);
        Acting = true;
        if (minDuration < 0.0f) _actDuration = Random.Range(_stats.actDurationMin, _stats.actDurationMax);
        else if (maxDuration < 0.0f) _actDuration = minDuration;
        else _actDuration = Random.Range(minDuration, maxDuration);
    }

    private void OnAct()
    {
        Debug.Log(nameof(OnAct));
        StartCoroutine(Act());
    }

    private IEnumerator Act()
    {
        TryActDamage();
        float remainingTime = _actDuration;
        while (remainingTime > 0.0f)
        {
            float tick = _stats.actDamageTick;
            yield return new WaitForSeconds(tick);
            TryActDamage();
            remainingTime -= tick;
        }

        OnActFinish();
    }

    private void TryActDamage()
    {
        Vector3 direction = _stats.playerTransform.position - transform.position;
        direction.Normalize();
        float angle = Vector3.Angle(transform.forward, direction);
        if (angle * 2.0f <= _stats.actFovAngle)
        {
            float alignment = Vector3.Dot(_stats.playerTransform.forward, -transform.forward);
            alignment = Mathf.Min(1.0f, alignment + Mathf.Cos(40.0f / 180.0f * Mathf.PI));
            if (alignment < 0.0f) return;
            _player.TakeActHit(_stats.actDamagePerTick * alignment);
        }
    }

    private void OnActFinish()
    {
        _animator.SetBool(AnimParamAct, false);
        Acting = false;
    }

    #endregion
}