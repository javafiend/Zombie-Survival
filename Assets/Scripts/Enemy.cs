using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour {

    private GameObject target;
    private NavMeshAgent agent;
    private Health health;
    private Animator animator;
    private Collider collider;
    private Health targetHealth;
    private Player player;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDead = false;
    public float speed = 1.0f;
    public float angularSpeed = 120;

    public float damage = 20;
    public float attackAngle = 45;
    [HideInInspector] public UnityEvent onDead;
    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        targetHealth = target.GetComponent<Health>();
        if (targetHealth == null)
        {
            throw new System.Exception("Target doesn't have Health Component");
        }
        player = target.GetComponent<Player>();
        if (player == null)
        {
            throw new System.Exception("Player doesn't have Player Component");
        }
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckHealth();
        Chase();
        CheckAttack();
    }

    void CheckHealth()
    {
        if (isDead)
            return;
        if(health.value <= 0)
        {
            onDead.Invoke();
            isDead = true;
            agent.isStopped = true;
            collider.enabled = false;
            animator.CrossFadeInFixedTime("Death", 0.1f);
            Destroy(gameObject, 3f);
        }
    }

    void Chase()
    {
        if (isDead)
            return;
        else if (player.isDead)
            return;
        agent.destination = target.transform.position;
    }

    void CheckAttack()
    {
        if (isDead)
            return;
        else if (isAttacking)
            return;
        else if (player.isDead)
            return;

        float distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);

        if (distanceFromTarget <= 1.8f)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(directionToTarget, transform.forward);
            if (angle <= attackAngle)
            {
                Attack();
            }

        }
    }

    void Attack()
    {
    
        targetHealth.TakeDamage(damage);
        agent.speed = 0;
        agent.angularSpeed = 0;
        isAttacking = true;
        animator.SetTrigger("ShouldAttack");
        Invoke("ResetAttacking", 1.5f);
    }

    void ResetAttacking()
    {
        isAttacking = false;
        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
    }
}
