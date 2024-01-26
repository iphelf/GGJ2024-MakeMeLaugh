using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerControl : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerStats _stats;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            HoldEars();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            ReleaseEars();
        if (Input.GetKeyDown(KeyCode.Space))
            CloseEyes();
        if (Input.GetKeyUp(KeyCode.Space))
            OpenEyes();
    }

    private void HandleMovement()
    {
        Vector3 direction = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (!Mathf.Approximately(direction.sqrMagnitude, 0.0f))
        {
            _controller.SimpleMove(_stats.speed * direction.normalized);
        }
    }

    private void HoldEars()
    {
        Debug.Log(nameof(HoldEars));
    }

    private void ReleaseEars()
    {
        Debug.Log(nameof(ReleaseEars));
    }

    private void CloseEyes()
    {
        Debug.Log(nameof(CloseEyes));
    }

    private void OpenEyes()
    {
        Debug.Log(nameof(OpenEyes));
    }

    public void TakeTickleHit(float damage)
    {
        _stats.currentHp = Mathf.Max(0.0f, _stats.currentHp - damage);
    }

    public void TakeJokeHit(float damage)
    {
        _stats.currentHp = Mathf.Max(0.0f, _stats.currentHp - damage);
    }
}