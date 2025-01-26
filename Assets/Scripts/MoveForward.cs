using UnityEngine;

public class MoveForward : MonoBehaviour
{
    // Speed of movement
    public float speed = 5f;

    void Update()
    {
        // Move the object forward along its local Z axis
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
