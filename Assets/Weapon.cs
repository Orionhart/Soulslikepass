using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Collider lightAttackCollider;

    public void EnableLightAttackHitBox()
    {
        lightAttackCollider.enabled = true;
    }
    public void DisableLightAttackHitBox()
    {
        lightAttackCollider.enabled = false;
    }
}
