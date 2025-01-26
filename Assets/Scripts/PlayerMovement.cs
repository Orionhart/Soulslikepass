using UnityEngine;
using UnityEngine.UI; // Import Unity UI namespace

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this value to change the movement speed
    public float maxHealth = 100f; // Maximum health value
    public float currentHealth; // Current health value

    public Slider healthSlider; // Reference to the UI slider for health

    private bool isAlive = true; // Flag to track whether the player is alive or dead

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health to max health
        UpdateHealthUI(); // Update UI to reflect initial health
    }

    void Update()
    {
        if (!isAlive)
            return; // If the player is dead, don't allow movement

        // Get the horizontal input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate the movement direction based on the input
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * moveSpeed * Time.deltaTime;

        // Apply the movement to the object's position
        transform.Translate(movement);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            // Decrease health when hit by an object with the "Attack" tag
            TakeDamage(10f); // You can adjust the amount of damage here
        }
    }

    void TakeDamage(float damage)
    {
        // Reduce health by the damage amount
        currentHealth -= damage;

        // Make sure health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0f);

        // Update UI to reflect new health
        UpdateHealthUI();

        // Check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death (e.g., respawn)
        Debug.Log("Player died!");
        isAlive = false; // Set the flag to indicate the player is dead
    }

    void UpdateHealthUI()
    {
        // Update the health slider's value to reflect current health
        healthSlider.value = currentHealth / maxHealth;
    }
}