using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerControl : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerStats _stats;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera closedCamera;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<PlayerStats>();
        PlayerCondition.eyesClosed = false;
        PlayerCondition.earsHeld = false;
        PlayerCondition.moving = false;
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

        DecreaseLaugh(_stats.laughAttenuation * Time.deltaTime);
        _stats.currentScore += _stats.gainRate.Evaluate(_stats.currentLaugh) * Time.deltaTime;
    }

    private void HandleMovement()
    {
        Vector3 direction = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        if (!Mathf.Approximately(direction.sqrMagnitude, 0.0f))
        {
            _controller.SimpleMove(_stats.speed * direction.normalized);
            transform.rotation = Quaternion.LookRotation(direction);
            PlayerCondition.moving = true;
        }
        else
            PlayerCondition.moving = false;
    }

    private void HoldEars()
    {
        Debug.Log(nameof(HoldEars));
        PlayerCondition.earsHeld = true;
    }

    private void ReleaseEars()
    {
        Debug.Log(nameof(ReleaseEars));
        PlayerCondition.earsHeld = false;
    }

    private void CloseEyes()
    {
        Debug.Log(nameof(CloseEyes));
        PlayerCondition.eyesClosed = true;
        mainCamera.enabled = false;
        closedCamera.enabled = true;
    }

    private void OpenEyes()
    {
        Debug.Log(nameof(OpenEyes));
        PlayerCondition.eyesClosed = false;
        closedCamera.enabled = false;
        mainCamera.enabled = true;
    }

    public void TakeTickleHit(float damage) => IncreaseLaugh(damage);

    public void TakeJokeHit(float damage)
    {
        if (PlayerCondition.earsHeld) return;
        IncreaseLaugh(damage);
    }

    public void TakeActHit(float damage)
    {
        if (PlayerCondition.eyesClosed) return;
        IncreaseLaugh(damage);
    }

    private void IncreaseLaugh(float laugh)
    {
        _stats.currentLaugh = Mathf.Min(_stats.maximumLaugh, _stats.currentLaugh + laugh);
    }

    private void DecreaseLaugh(float laugh)
    {
        _stats.currentLaugh = Mathf.Max(0.0f, _stats.currentLaugh - laugh);
    }
}