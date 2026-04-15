using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput;
    public bool JumpPressed;

    private PlayerInputActions _input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _input.Enable();

        _input.Player.Jump.performed += _ => JumpPressed = true;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Player.Jump.performed -= _ => JumpPressed = true;
    }

    private void Update()
    {
        MoveInput = _input.Player.Move.ReadValue<Vector2>();
    }

    public void ResetJump()
    {
        JumpPressed = false;
    }
}
