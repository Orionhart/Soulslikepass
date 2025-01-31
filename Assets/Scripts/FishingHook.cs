using DG.Tweening;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    
    public void CastTo(Vector3 position)
    {
        rigidBody.AddRelativeForce(position, ForceMode.Impulse);
    }

    public void Unparent()
    {
        transform.parent = null;
        rigidBody.isKinematic = false;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.parent = other.gameObject.transform;
            rigidBody.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //transform.parent = null;
            //rigidBody.isKinematic = false;
        }
    }
}
