using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Universal/Generic component to allow objects to deal damage to Health components
/// Only applies damage to objects with the required tags
/// </summary>
public class DamageSource : MonoBehaviour
{
    #region----- VARIABLES -----

    [Tooltip("Damage to deal. NOTE: negative values will heal")]
    public float dmg = 1;
    public List<string> tagsToDamage = new();
    [SerializeField] bool DestroyOnHit;
    [SerializeField] GameObject hitEffects;

    #endregion

    #region----- MONOBEHAVIOURS -----

    private void OnTriggerEnter(Collider other)
    {
        if (tagsToDamage.Contains(other.tag))
        {
            Debug.Log($"{name} -> {other.name}");
            Health.OnDamage(other.gameObject, dmg);
            PlayerScript.OnDamage(other.gameObject, dmg);
            if (DestroyOnHit)
            {
                Destroy(gameObject);
                if (hitEffects != null)
                    Instantiate(hitEffects, transform.position, Quaternion.identity);
            } 
        }
    }

    #endregion

    #region----- CUSTOM BEHAVIOURS -----

    /// <summary> Sets the tags the DamageSource will check for. Useful for when tags change at runtime e.g bullets </summary>
    /// <param name="tags"> The tags to check for </param>
    public void SetDamageTags(params string[] tags)
    {
        tagsToDamage = new List<string>(tags);
    }


    #endregion
}
