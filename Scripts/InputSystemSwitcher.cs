using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class InputSystemSwitcher : MonoBehaviour
{
    public InputSystemUIInputModule uiInputModule;

    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string mousePointAction = "MousePoint";
    [SerializeField] private string touchPointAction = "TouchPoint";

    public void SetMouseInput()
    {
        var action = inputActions.FindAction(mousePointAction);
        if (action != null)
        {
            uiInputModule.point = InputActionReference.Create(action);
            Debug.Log("Switched to Mouse Position input.");
        }
    }

    public void SetTouchInput()
    {
        var action = inputActions.FindAction(touchPointAction);
        if (action != null)
        {
            uiInputModule.point = InputActionReference.Create(action);
            Debug.Log("Switched to Touch Position input.");
        }
    }

    public void DisablePointInput()
    {
        uiInputModule.point = null;
        Debug.Log("Point input disabled.");
    }
}
