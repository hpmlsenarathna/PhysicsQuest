using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    private InputAction mousePosAction;
    private InputAction mouseAction;

    public static Vector2 mousePos;

    public static bool WasLeftMosueButtonPressed;
    public static bool IsLeftMousePressed;
    public static bool WasLeftMosueButtonReleased;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        mousePosAction = PlayerInput.actions["MousePosition"];
        mouseAction = PlayerInput.actions["Touch"];
    }

    private void Update()
    {
        mousePos = mousePosAction.ReadValue<Vector2>();
        WasLeftMosueButtonPressed = mouseAction.WasPressedThisFrame();
        WasLeftMosueButtonReleased = mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = mouseAction.IsPressed();
    }
}
