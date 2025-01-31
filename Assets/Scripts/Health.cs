using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Universal/generic health component that handles recieving damage from DamageSource components.
/// Uses UnityEvents to invoke custom behaviours when the object is healed, damaged or killed.
/// </summary>
public class Health : MonoBehaviour
{
    #region----- STATICS/MISC -----

    public delegate void DamageHandler(GameObject hitObj, float dmg, ElementalAffinity element);
    public static event DamageHandler Damage;
    public static void OnDamage(GameObject hitObj, float dmg, ElementalAffinity element) => Damage?.Invoke(hitObj, dmg, element);

    #endregion

    #region----- VARIABLES -----
    public ElementalAffinity myElement;
    [SerializeField] float _current = 1;
    public float current
    {
        get => _current;
        set
        {
            _current = value;
            if (_current < 0)
                _current = indestructable ? 1 : 0;
            if ((_current > maxHealth) && (maxHealth > 0))
                _current = maxHealth;
        }
    }
    public float maxHealth;
    public bool indestructable = false;
    public bool debugModify = false;

    // Perform any specific behavious with unity events
    public UnityEvent OnHeal;
    public UnityEvent OnHit;
    [Tooltip("OnDynamicHit can be used to pass on the specific damage value if needed")]
    public UnityEvent<float> OnDynamicHit;
    public UnityEvent OnDeath;

    #endregion

    #region----- MONOBEHAVIOURS -----

    void OnEnable()
    {
        Damage += TakeDamage;
    }

    void OnDisable()
    {
        Damage -= TakeDamage;
    }

    #endregion

    #region----- CUSTOM BEHAVIOURS -----

    public void TakeDamage(GameObject hitObj, float dmg, ElementalAffinity element)
    {
        if (hitObj != this.gameObject)
            return;

        ModifyHealth(GetElementDamageModified(dmg, element) * -1);
    }

    float GetElementDamageModified(float dmg, ElementalAffinity element)
    {
        return dmg * AffinityCoefficient(myElement, element);
    }

    float AffinityCoefficient(ElementalAffinity receiving, ElementalAffinity dealing)
    {
        switch (receiving)
        {
            case ElementalAffinity.Electric:
                switch (dealing)
                {
                    case ElementalAffinity.Electric:
                        return .75f;
                    case ElementalAffinity.Fire:
                        return .5f;
                    case ElementalAffinity.Ice:
                        return 2f;
                    case ElementalAffinity.None:
                        return 1f;
                }
                break;
            case ElementalAffinity.Fire:
                switch (dealing)
                {
                    case ElementalAffinity.Electric:
                        return 2f;
                    case ElementalAffinity.Fire:
                        return .75f;
                    case ElementalAffinity.Ice:
                        return .5f;
                    case ElementalAffinity.None:
                        return 1f;
                }
                break;
            case ElementalAffinity.Ice:
                switch (dealing)
                {
                    case ElementalAffinity.Electric:
                        return .5f;
                    case ElementalAffinity.Fire:
                        return 2f;
                    case ElementalAffinity.Ice:
                        return .75f;
                    case ElementalAffinity.None:
                        return 1f;
                }
                break;
            case ElementalAffinity.None:
                return 1;
        }

        return 1;
    }

    public void ModifyHealth(float mod)
    {
        if (debugModify)
            Debug.Log($"{name}: {current} += {mod}");

        current += mod;
        if (mod > 0)
            OnHeal.Invoke();
        else if (current > 0)
        {
            OnHit.Invoke();
            OnDynamicHit.Invoke(mod);
        }
        else
            OnDeath.Invoke();
        
    }

    #endregion
}
