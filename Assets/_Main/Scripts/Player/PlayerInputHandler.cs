using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput;

    public event Action OnJump;
    public event Action OnInteract;

    private PlayerInputActions _input;

    private void Awake()
    {
        _input = new PlayerInputActions();
    }
    private void Update()
    {
        MoveInput = _input.Player.Move.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.performed += HandleJump;
        _input.Player.Interact.performed += HandleInteract;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Player.Jump.performed -= HandleJump;
        _input.Player.Interact.performed -= HandleInteract;
    }

    private void HandleJump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke();
    }

    private void HandleInteract(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }
}
