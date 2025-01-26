using UnityEngine;

public class GamepadDetection : MonoBehaviour
{
    private string[] previousJoystickNames;

    void Start()
    {
        // Initialize the previousJoystickNames array with the current joystick names
        previousJoystickNames = Input.GetJoystickNames();
    }

    void Update()
    {
        // Get the current joystick names
        string[] currentJoystickNames = Input.GetJoystickNames();

        // Check if there's a change in the connected joysticks
        if (!AreArraysEqual(previousJoystickNames, currentJoystickNames))
        {
            Debug.Log("Gamepad status changed:");

            // Log the current joystick names
            for (int i = 0; i < currentJoystickNames.Length; i++)
            {
                Debug.Log("Joystick " + (i + 1) + ": " + currentJoystickNames[i]);
            }

            // Update the previousJoystickNames array with the current joystick names
            previousJoystickNames = currentJoystickNames;
        }

        // Check for input from the first joystick
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Check if any input is coming from the joystick
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            Debug.Log("Joystick input detected: Horizontal = " + horizontalInput + ", Vertical = " + verticalInput);
        }
    }

    // Helper function to check if two string arrays are equal
    bool AreArraysEqual(string[] array1, string[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i] != array2[i])
                return false;
        }

        return true;
    }
}
