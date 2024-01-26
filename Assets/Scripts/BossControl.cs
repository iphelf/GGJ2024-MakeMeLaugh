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

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _stats = GetComponent<BossStats>();
        _cameraTransform = Camera.main!.transform;

        tickler.OnHitPlayer += OnTickleHit;
    }

    public void LookAt(Vector3 position)
    {
        transform.rotation = Quaternion.LookRotation(position - transform.position);
    }

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        _controller.SimpleMove(_stats.speed * direction.normalized);
    }

    public bool NoAction => !Tickling && !Joking;

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
}