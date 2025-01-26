using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayerWithTag : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player object
    public float moveSpeed = 5f; // Speed at which the object moves towards the player
    public float delayTime = 3f; // Time in seconds before stopping

    private Transform playerTransform; // Reference to the player's transform
    private bool shouldMove = true; // Flag indicating if the object should move towards the player

    private void Start()
    {
        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // Start the delay timer
        Invoke("StopMoving", delayTime);
    }

    private void Update()
    {
        if (shouldMove && playerTransform != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = playerTransform.position - transform.position;
            direction.Normalize(); // Normalize the direction vector to have a magnitude of 1

            // Move towards the player
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void StopMoving()
    {
        shouldMove = false;
    }
}