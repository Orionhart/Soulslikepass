using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Collider lightAttackCollider;
    [SerializeField] private Collider heavyAttackCollider;

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
