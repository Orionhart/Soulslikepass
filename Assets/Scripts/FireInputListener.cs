using UnityEngine;

public class FireInputListener : MonoBehaviour
{
    void Update()
    {
        // Check if Fire1 button is pressed
        if (Input.GetButtonDown("Fire1"))
        {
            // Print message to the console
            Debug.Log("Fire1 button pressed!");
        }
    }
}
