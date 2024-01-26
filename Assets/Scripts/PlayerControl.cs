using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    private CharacterController _controller;
    public float speed = 5.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
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
            _controller.SimpleMove(speed * direction.normalized);
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
}