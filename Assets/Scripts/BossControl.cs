using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class BossControl : MonoBehaviour
{
    public float speed = 4.0f;
    private CharacterController _controller;
    private Animator _animator;
    private static readonly int AnimParamTickle = Animator.StringToHash("Tickle");

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    public void Move(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        _controller.SimpleMove(speed * direction.normalized);
    }

    public bool NoAction => !Tickling;

    public bool Tickling { get; private set; }

    public void BeginTickle()
    {
        _animator.SetTrigger(AnimParamTickle);
        Tickling = true;
    }

    public void OnTickle()
    {
    }

    public void OnTickleFinish()
    {
        Tickling = false;
    }
}