using System;
using DG.Tweening;
using UnityEngine;

public class FishingHook : MonoBehaviour
{
    [SerializeField] Rigidbody rigidBody;
    public ElementalAffinity myElement;
    public float damage = 1f;

    private Health currentEnemyHealth = null;
    
    /// <summary>
    /// Trying to make the fishing rod cast to a point or in an arc... or something.
    /// </summary>
    /// <param name="position"></param>
    public void CastTo(Vector3 position)
    {
        rigidBody.AddRelativeForce(position, ForceMode.Impulse);
    }

    public void Unparent()
    {
        transform.parent = null;
        rigidBody.isKinematic = false;
    }
    
    /// <summary>
    /// Hook sticks into enemies when it hits and deals elemental damage based on the current element.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.parent = other.gameObject.transform;
            
            other.gameObject.TryGetComponent(out currentEnemyHealth);
            
            if (currentEnemyHealth != null)
            {
                currentEnemyHealth.TakeDamage(other.gameObject, damage, myElement);
            }
            
            rigidBody.isKinematic = true;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (currentEnemyHealth != null)
        {
            currentEnemyHealth.TakeDamage(other.gameObject, damage * Time.deltaTime, myElement);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            transform.parent = null;
            rigidBody.isKinematic = false;
            currentEnemyHealth = null;
        }
    }
}
