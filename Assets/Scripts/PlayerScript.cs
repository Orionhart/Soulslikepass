using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using NUnit.Framework;


public enum PlayerState { normal, dashing, stunned, testing, attacking, resting }
public enum AttackType { light, heavy }
public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    
    public delegate void DamageHandler(GameObject hitObj, float dmg);
    public static event DamageHandler Damage;
    public static void OnDamage(GameObject hitObj, float dmg) => Damage?.Invoke(hitObj, dmg);

    public static bool isPaused => Instance.paused;

    public bool paused = false;

    [SerializeField] LayerMask mouseColliderLayerMask;
    [SerializeField] GameObject parryEffect;
    bool parrying = false;
    [SerializeField] GameObject[] powerCharacterEffects;
    [SerializeField] SkinnedMeshRenderer characterRenderer;
    [SerializeField] Material normalMat;
    [SerializeField] Material invincibleMat;
    [SerializeField] Transform bullet;
    [SerializeField] Material hurtMat;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float sprintSpeed = 6;
    [SerializeField] float baseHealth;
    [SerializeField] Transform spawnBulletTransform;
    [SerializeField] Animator weaponAnimator;
    [SerializeField] Weapon currWeapon;
    int parryLevel;

    float invincibleTimer = 1f;
    float sprintCost = .5f;
    float dashCost = 15f;
    float lightAttackCost = 5f;
    public float jumpCost = 9f;
    float heavyAttackCost = 12f;

    public bool isSitting = false;
    //HUD PlayerHUD;
    public Health HealthObject;
    public Stamina StaminaObject;
    public StarterAssetsInputs starterAssetsInputs;
    ThirdPersonController thirdPersonController;
    Animator animator;
    [SerializeField] PlayerState state = PlayerState.normal;
    bool noButtons = true;

    AttackType currAttackType = AttackType.light;

    private void Awake()
    {
        Instance = this;
    }

    //todo: add stamina system
    private void Start()
    {
        GetComponent<PlayerInput>().enabled = true;
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        HealthObject = GetComponent<Health>();
        //PlayerHUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>();
        HealthObject.current = baseHealth;
        animator = GetComponent<Animator>();
        parryLevel = 0;
    }
    void OnEnable()
    {
        Damage += TakeHit;
    }

    void OnDisable()
    {
        Damage -= TakeHit;
    }
    void TakeHit(GameObject hitObj, float dmg)
    {
        if (hitObj != this.gameObject)
            return;
        if (parrying)
        {
            Instantiate(parryEffect, transform.position + new Vector3(0, 1, 0), transform.rotation);
            parryLevel++;
        }
        else
        {
            state = PlayerState.stunned;
            animator.Play("Stunned", 0);
            SetInvincible(1f, true);
            parryLevel = 0;
        }
    }

    public void Jumped()
    {
        StaminaObject.ModifyStamina(-1 * jumpCost, 1f);
    }

    public void TriggerRest()
    {
        animator.Play("Resting", 0);
        state = PlayerState.resting;
        HealthObject.ModifyHealth(HealthObject.maxHealth - HealthObject.current);
        StaminaObject.ModifyStamina(StaminaObject.maxStamina - StaminaObject.current);
        paused = true;
        isSitting = true;
        parryLevel = 0;
    }

    public void TriggerNotRest()
    {
        animator.Play("GetUp", 0);
        paused = false;
        isSitting = false;
        state = PlayerState.normal;
    }

    public void Dead()
    {
        SceneManager.LoadScene("GameOver");
    }


    void SetInvincible(float IFramesTime, bool hurt = false)
    {
        invincibleTimer = IFramesTime;
        GetComponent<Health>().enabled = false;
        if (hurt)
            characterRenderer.material = hurtMat;
        else
            characterRenderer.material = invincibleMat;
    }

    void Update()
    {
        if (starterAssetsInputs.pause)
        {
            Pause();
        }

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }
        
        powerCharacterEffects[0].SetActive(parryLevel > 0);
        powerCharacterEffects[1].SetActive(parryLevel > 1);
        powerCharacterEffects[2].SetActive(parryLevel > 2);

        invincibleTimer -= Time.deltaTime;
        if (invincibleTimer < 0f)
        {
            characterRenderer.material = normalMat;
            GetComponent<Health>().enabled = true;
            parrying = false;
        }

        Cursor.lockState = CursorLockMode.Locked;

        switch (state)
        {
            case PlayerState.normal:
                //should make running drain stamina, if i had stamina?
                Normal();
                break;
            case PlayerState.dashing:
                Dash();
                break;
            case PlayerState.stunned:
                characterRenderer.material = hurtMat;
                thirdPersonController.AllowMovement(false);
                thirdPersonController.SetRotateOnMove(false);
                //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
                break;
            case PlayerState.testing:
                thirdPersonController.AllowMovement(false);
                thirdPersonController.SetRotateOnMove(false);
                break;
            case PlayerState.resting:
                thirdPersonController.AllowMovement(false);
                thirdPersonController.SetRotateOnMove(false);
                break;
        }
    }

    void LateUpdate()
    {

        if (paused)
        {
            return;
        }
        
        HealthObject.maxHealth = baseHealth;
        //PlayerHUD.SetHP(HealthObject.current, HealthObject.maxHealth);
        switch (state)
        {
            case PlayerState.dashing:
                break;
            case PlayerState.normal:
                break;
            case PlayerState.stunned:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
                    state = PlayerState.normal;
                break;
            case PlayerState.testing:
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    state = PlayerState.normal;
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
    }

    void Shoot(Vector3 aimDir, Vector3 spawnPosition, float dam)
    {
        parryLevel = 0;
        var _o = Instantiate(bullet, spawnPosition, Quaternion.LookRotation(aimDir, Vector3.up));
        /*DamageSource[] _ds = _o.GetComponents<DamageSource>();
        foreach (var item in _ds)
            item.dmg += dam;*/
    }

    void Normal()
    {
        
        if ((starterAssetsInputs.sprint == false) && (starterAssetsInputs.dash == false) && (starterAssetsInputs.jump == false) && (thirdPersonController.Grounded == true) && (starterAssetsInputs.move.magnitude <= 0.1f))
        {
            if ((noButtons == false) && (invincibleTimer < 0f))
            {
                noButtons = true;
                parrying = true;
                StaminaObject.StopStaminaRegain();
                SetInvincible(1f);
            }
        }
        else
            noButtons = false;


        if (starterAssetsInputs.sprint)
        {
            StaminaObject.ModifyStamina(Time.deltaTime * dashCost * -1f, .1f);
        }
        
        
        invincibleTimer -= Time.deltaTime;

        if (invincibleTimer < 0f)
        {
            thirdPersonController.SprintSpeed = Mathf.Lerp(thirdPersonController.SprintSpeed, sprintSpeed, Time.deltaTime * 5f);
            thirdPersonController.MoveSpeed = Mathf.Lerp(thirdPersonController.MoveSpeed, moveSpeed, Time.deltaTime * 5f);
        }

        thirdPersonController.AllowMovement(true);
        thirdPersonController.SetRotateOnMove(true);
        //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

        if ((starterAssetsInputs.action) && (parryLevel > 0))
        {
            Vector3 mouseWorldPosition = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
                mouseWorldPosition = ray.origin + ray.direction * 20f;
            Vector3 aimDir = (mouseWorldPosition - spawnBulletTransform.position).normalized;
            Shoot(aimDir, spawnBulletTransform.position, 0);
            starterAssetsInputs.action = false;
        }

        if (starterAssetsInputs.dash && StaminaObject.current > dashCost)
        {
            //should have stamina cost
            state = PlayerState.dashing;
            thirdPersonController.SprintSpeed = 20f;
            thirdPersonController.MoveSpeed = 20f;
            starterAssetsInputs.dash = false;
            StaminaObject.ModifyStamina(dashCost * -1);
        }

        if (starterAssetsInputs.lightAttack && StaminaObject.current > lightAttackCost)
        {
            state = PlayerState.attacking;
            thirdPersonController.AllowMovement(false);
            thirdPersonController.AllowRotate(false);
            animator.SetTrigger("LightAttack");
            if(weaponAnimator) weaponAnimator.SetTrigger("LightAttack");
            starterAssetsInputs.lightAttack = false;
            StaminaObject.ModifyStamina(lightAttackCost * -1);
        }
        if (starterAssetsInputs.heavyAttack && StaminaObject.current > heavyAttackCost)
        {
            state = PlayerState.attacking;
            thirdPersonController.AllowMovement(false);
            thirdPersonController.AllowRotate(false);
            animator.SetTrigger("HeavyAttack");
            if(weaponAnimator) weaponAnimator.SetTrigger("HeavyAttack");
            starterAssetsInputs.heavyAttack = false;
            StaminaObject.ModifyStamina(heavyAttackCost * -1);
        }
    }

    public void EnableLightAttackHitBox()
    {
        currWeapon.EnableLightAttackHitBox();
    }

    public void DisableLightAttackHitBox()
    {
        currWeapon.DisableLightAttackHitBox();
    }
    
    public void EnableHeavyAttackHitBox()
    {
        currWeapon.EnableHeavyAttackHitBox();
    }

    public void DisableHeavyAttackHitBox()
    {
        currWeapon.DisableHeavyAttackHitBox();
    }

    // called by animation to determine when to leave the attacking state
    public void FinishAttacking()
    {
        Debug.Log("Finished Attacking");
        state = PlayerState.normal;
        thirdPersonController.AllowMovement(true);
        thirdPersonController.AllowRotate(true);
    }

    void Dash()
    {
        
        GetComponent<Health>().enabled = true;
        thirdPersonController.Gravity = -10;
        thirdPersonController.SetRotateOnMove(true);
        thirdPersonController.AllowMovement(true);
        //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

        thirdPersonController.SprintSpeed = Mathf.Lerp(thirdPersonController.SprintSpeed, sprintSpeed, Time.deltaTime * 5f);
        thirdPersonController.MoveSpeed = Mathf.Lerp(thirdPersonController.MoveSpeed, moveSpeed, Time.deltaTime * 5f);
        if (Mathf.Abs(thirdPersonController.SprintSpeed - sprintSpeed) < 0.1f)
        {
            thirdPersonController.Gravity = -15;
            state = PlayerState.normal;
        }

        if (starterAssetsInputs.action)
            starterAssetsInputs.action = false;
    }
    [SerializeField] GameObject pauseMenu = null;

    public void Pause()
    {
        starterAssetsInputs.pause = false;
        if (CheckpointUI.Instance.isOpen)
        {
            return;
        }
        Debug.Log("Pause " + paused);
        paused = !paused;
        if (paused)
        {
            thirdPersonController.AllowMovement(false);
            thirdPersonController.SetRotateOnMove(false);
        }
        else
        {
            thirdPersonController.AllowMovement(true);
            thirdPersonController.SetRotateOnMove(true);
        }
        Time.timeScale = paused ? 0 : 1;
        if(pauseMenu) pauseMenu.SetActive(paused);
    }
}