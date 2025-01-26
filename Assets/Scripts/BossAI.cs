using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{

    public GameObject[] attackList;
    public string[] attackAnimations;
    public float attackCooldown = 2f;
    CharacterController player;
    NavMeshAgent navMeshAgent;
    private Animator animator;
    private float timer = 0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindFirstObjectByType<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        navMeshAgent.destination = player.transform.position;

        if (
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack0") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack4") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack5"))
        {
            navMeshAgent.speed = 3f;
        }

        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            CreateRandomAttack();
            timer = 0f;
        }

    }

    private void CreateRandomAttack()
    {
        navMeshAgent.speed = 0f;
        int randomAttackI = Random.Range(0, attackList.Length);
        animator.Play(attackAnimations[randomAttackI]);
        GameObject newObject = Instantiate(attackList[randomAttackI], transform.position, transform.rotation);
        newObject.transform.rotation = transform.rotation;
    }
}