using UnityEngine;

public enum WeaponElement { None, Electric, Fire, Ice}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponElement element = WeaponElement.None;
    
    [SerializeField] private FishingHook hook;
    
    [SerializeField] private Collider lightAttackCollider;
    [SerializeField] private Collider heavyAttackCollider;
    
    [SerializeField] private DamageSource lightDamageSource;
    [SerializeField] private DamageSource heavyDamageSource;

    [SerializeField] private GameObject iceParticleObject;
    [SerializeField] private GameObject fireParticleObject;
    [SerializeField] private GameObject electricParticleObject;

    public void ChangeElement(WeaponElement element)
    {
        this.element = element;
        
        electricParticleObject.SetActive(false);
        fireParticleObject.SetActive(false);
        iceParticleObject.SetActive(false);

        switch (element)
        {
            case WeaponElement.Electric:
                electricParticleObject.SetActive(true);
                break;
            case WeaponElement.Fire:
                fireParticleObject.SetActive(true);
                break;
            case WeaponElement.Ice:
                iceParticleObject.SetActive(true);
                break;
        }
        
        lightDamageSource.element = element;
        heavyDamageSource.element = element;
    }

    public void EnableLightAttackHitBox()
    {
        lightAttackCollider.enabled = true;
    }
    
    public void DisableLightAttackHitBox()
    {
        lightAttackCollider.enabled = false;
    }
    
    public void EnableHeavyAttackHitBox()
    {
        lightAttackCollider.enabled = true;
    }
    
    public void DisableHeavyAttackHitBox()
    {
        lightAttackCollider.enabled = false;
    }
}
