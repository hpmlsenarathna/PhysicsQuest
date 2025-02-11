using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    // Components
    TopDownCarController topDownCarController;

    // Track if mobile control is active
    private bool isMobileControlActive = false;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMobileControlActive) // Keyboard input only when mobile control is inactive
        {
            // Check for keyboard input and map to car movement (for arrow keys)
            if (Input.GetKey(KeyCode.UpArrow))
            {
                topDownCarController.MoveUp();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                topDownCarController.MoveDown();
            }
            else
            {
                topDownCarController.StopAcceleration(); // Reset if no key is pressed
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                topDownCarController.TurnLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                topDownCarController.TurnRight();
            }
            else
            {
                topDownCarController.StopSteering(); // Reset steering when no key is pressed
            }
        }
    }

    // Methods for mobile button controls
    public void OnMoveUpButtonPressed()
    {
        Debug.Log("Move Up Button Pressed"); // Debugging
        isMobileControlActive = true;        // Activate mobile control
        topDownCarController.MoveUp();       // Call MoveUp method in TopDownCarController
    }

    public void OnMoveDownButtonPressed()
    {
        Debug.Log("Move Down Button Pressed"); // Debugging
        isMobileControlActive = true;          // Activate mobile control
        topDownCarController.MoveDown();       // Call MoveDown method in TopDownCarController
    }

    public void OnLeftButtonPressed()
    {
        Debug.Log("Left Button Pressed");
        isMobileControlActive = true;
        topDownCarController.TurnLeft();
    }

    public void OnRightButtonPressed()
    {
        Debug.Log("Right Button Pressed");
        isMobileControlActive = true;
        topDownCarController.TurnRight();
    }

    // Stop actions when button is released
    public void OnStopAcceleration()
    {
        Debug.Log("Stop Acceleration");
        topDownCarController.StopAcceleration();
    }

    public void OnStopSteering()
    {
        Debug.Log("Stop Steering");
        topDownCarController.StopSteering();
    }
}