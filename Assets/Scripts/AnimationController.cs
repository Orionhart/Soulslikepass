using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public string[] animationNames; // Array of animation names
    private int idleStateHash; // Hash of the idle state
    private int attackStateHash; // Hash of the attack state

    // Range for random idle duration
    public float minIdleDuration = 1f;
    public float maxIdleDuration = 3f;

    // Movement speed while in idle state
    public float moveSpeed = 3f;

    void Start()
    {
        // Cache the hash of the idle and attack states for efficiency
        idleStateHash = Animator.StringToHash("Idle");
        attackStateHash = Animator.StringToHash("Attack");

        // Start playing the animation loop
        StartCoroutine(PlayAnimationLoop());
    }

    IEnumerator PlayAnimationLoop()
    {
        while (true)
        {
            // Pick a random animation from the array
            string randomAnimation = animationNames[Random.Range(0, animationNames.Length)];

            // Play the picked animation
            animator.Play(randomAnimation);

            // Wait for the animation to finish playing
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Randomly wait in idle state
            float idleDuration = Random.Range(minIdleDuration, maxIdleDuration);
            yield return new WaitForSeconds(idleDuration);

            // While in idle state, move towards the nearest player's x position
            GameObject nearestPlayer = FindNearestPlayer();
            if (nearestPlayer != null)
            {
                // Get the direction towards the player along the X-axis only
                Vector3 targetPosition = nearestPlayer.transform.position;
                targetPosition.y = transform.position.y; // Keep the same height
                Vector3 moveDirection = (targetPosition - transform.position).normalized;
                moveDirection.z = 0f; // Ensure no movement along the z-axis

                // Move towards the player along the X-axis
                while (Mathf.Abs(transform.position.x - targetPosition.x) > 0.1f)
                {
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;

                    yield return null; // Wait for the next frame
                }
            }
        }
    }

    GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;
    }
}
