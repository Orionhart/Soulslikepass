using StarterAssets;
using UnityEngine;
using UnityEngine.Playables;

public enum ElementalAffinity { None, Electric, Fire, Ice}

public class Weapon : MonoBehaviour
{
    [SerializeField] private ElementalAffinity element = ElementalAffinity.None;
    
    [SerializeField] private FishingHook hook;

    [SerializeField] private float maxCastDistanceLight = 5f;
    [SerializeField] private float maxCastDistanceHeavy = 15f;
    
    [SerializeField] private Collider lightAttackCollider;
    [SerializeField] private Collider heavyAttackCollider;
    
    [SerializeField] private DamageSource lightDamageSource;
    [SerializeField] private DamageSource heavyDamageSource;

    [SerializeField] private GameObject iceParticleObject;
    [SerializeField] private GameObject fireParticleObject;
    [SerializeField] private GameObject electricParticleObject;

    public void ChangeElement(ElementalAffinity element)
    {
        this.element = element;
        
        electricParticleObject.SetActive(false);
        fireParticleObject.SetActive(false);
        iceParticleObject.SetActive(false);

        switch (element)
        {
            case ElementalAffinity.Electric:
                electricParticleObject.SetActive(true);
                break;
            case ElementalAffinity.Fire:
                fireParticleObject.SetActive(true);
                break;
            case ElementalAffinity.Ice:
                iceParticleObject.SetActive(true);
                break;
        }
        
        lightDamageSource.element = element;
        heavyDamageSource.element = element;
    }

    public void OnLightAttack()
    {
        hook.Unparent();
        
        if (hook.transform.parent == null)
        {
            hook.CastTo(Vector3.ClampMagnitude(PlayerScript.Instance.transform.forward, maxCastDistanceLight));
        }
    }

    public void OnHeavyAttack()
    {
        hook.Unparent();
        
        if (hook.transform.parent == null)
        {
            hook.CastTo(Vector3.ClampMagnitude(PlayerScript.Instance.transform.forward, maxCastDistanceHeavy));
        }
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
