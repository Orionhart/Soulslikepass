using UnityEngine;
using TMPro;

public class CountdownScript : MonoBehaviour
{
    public float countdownTime = 300f; // Countdown time in seconds (5 minutes)
    public TextMeshProUGUI countdownText; // Reference to the TextMeshPro text element
    public GameObject objectToCreate; // Reference to the prefab of the object to create

    private float currentTime;
    private bool countdownFinished = false;

    private void Start()
    {
        currentTime = countdownTime;
    }

    private void Update()
    {
        if (countdownFinished)
            return;

        currentTime -= Time.deltaTime;

        // Update the countdown text
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (currentTime <= 0f)
        {
            countdownFinished = true;
            countdownText.text = "Boss Spawned";

            // Create the object
            Instantiate(objectToCreate, transform.position, Quaternion.identity);
        }
    }
}



