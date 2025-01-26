using UnityEngine;

public class AnimationEventScript : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Ensure spawn point is set, if not, use this GameObject's position
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    // This method will be called by animation events
    public void SpawnPrefab()
    {
        // Check if the prefab to spawn is assigned
        if (prefabToSpawn != null && spawnPoint != null)
        {
            // Spawn the prefab at the specified spawn point's position and rotation
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Prefab to spawn or spawn point not assigned!");
        }
    }

    // This method can be used to trigger the animation event externally if needed
    public void TriggerAnimationEvent()
    {
        animator.SetTrigger("EventTrigger"); // Replace "EventTrigger" with the name of your animation event
    }
}