using UnityEngine;
using UnityEngine.Events;

public class Stamina : MonoBehaviour
{
    #region----- VARIABLES -----

    [SerializeField] float _current = 1;
    public float current
    {
        get => _current;
        set
        {
            _current = value;
        }
    }
    public float maxStamina;
    public float staminaRegainTimer = 0;
    public float staminaRegainMultiplier = 1;
    public bool staminaRegainBlocked = false;
    public bool debugModify = false;

    // Perform any specific behavious with unity events
    public UnityEvent<float, float> OnStaminaChanged = new UnityEvent<float, float>();
    #endregion
    
    #region----- UNITY METHODS -----

    public void Update()
    {
        if (staminaRegainTimer > 0)
        {
            staminaRegainTimer -= Time.deltaTime;
            staminaRegainBlocked = true;
        }
        else
        {
            staminaRegainTimer = 0;
            staminaRegainBlocked = false;
        }

        if (!staminaRegainBlocked && current < maxStamina)
        {
            ModifyStamina(Time.deltaTime * staminaRegainMultiplier, 0);
        }
    }
    #endregion
    
    #region----- CUSTOM BEHAVIOURS -----

    public void IncreaseMaxStamina(float mod)
    {
        if (debugModify)
            Debug.Log($"{name}: {current} += {mod}, {maxStamina} += {mod}");
        
        maxStamina += mod;
        current += mod;
        
        OnStaminaChanged?.Invoke(current, maxStamina);
    }
    
    public void ModifyStamina(float mod, float staminaRegainDelay = 1)
    {
        if (debugModify)
            Debug.Log($"{name}: {current} += {mod}");

        if (mod < 0)
        {
            StopStaminaRegain(staminaRegainDelay);
        }
        
        current += mod;
        if (current >= maxStamina)
        {
            current = maxStamina;
        }

        if (current < 0)
        {
            current = 0;
        }
        OnStaminaChanged?.Invoke(current, maxStamina);
    }

    public void StopStaminaRegain(float time = 0.1f)
    {
        staminaRegainTimer = time;
    }
    #endregion
}
