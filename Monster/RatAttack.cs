using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RatAttack : MonoBehaviour
{
    public int damage = 10;
    public GameObject player;
    private PlayerAudio playerAudio;

    private Animator anim;
    private PlayerHealth playerHealth;
    //private MonsterHealth monsterHealth;
    private bool inRange = false;
    private bool chasing = false;
    private bool endState = false;
    private float distanceToPlayer = 0.0f;
    private NavMeshAgent agent;

    private Coroutine playDamage = null;
    public bool attacking = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        //monsterHealth = GetComponent<MonsterHealth>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerAudio = player.GetComponent<PlayerAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        //first check if rat is chasing, then check distance.
        if (chasing && playerHealth.health > 0)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= 2.0f)
                inRange = true;
            else
            {
                inRange = false;
                attacking = false;
                agent.isStopped = false;
            }

            if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Attack") && inRange &&
            !anim.GetAnimatorTransitionInfo(1).IsName("New State -> Attack"))
                Attack();
        }

        if (playerHealth.health <= 0)
        {
            anim.SetBool("PlayerDead", true);
            if (playDamage != null)
                StopCoroutine(playDamage);
        }
    }


    //Could use some work
    //Improvements: Add smoothing instead of agent.isStopped = true / false.
    private void Attack()
    {
        if (!chasing || endState)
            return;

        attacking = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        agent.isStopped = true;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 4);
        if (hit.collider == null)
        {
            Quaternion targetRot = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 3.0f);
            rb.MoveRotation(targetRot);
        }
        else if (hit.collider.tag != "Player")
        {
            Quaternion targetRot = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
            rb.MoveRotation(targetRot);
        }

        anim.SetTrigger("Attack");

        if (playerHealth.health > 0)
        {
            StartCoroutine(AttackAnimDelay());
            playDamage = StartCoroutine(playerAudio.PlayDamage());
        }
    }

    //Use this to sync damage with the attack animation
    IEnumerator AttackAnimDelay()
    {
        yield return new WaitForSeconds(0.29f);
        playerHealth.TakeDamage(damage);
    }

    //Linked to event in WanderingAI Script.
    public void OnStartChase()
    {
        chasing = true;
    }

    public void PlayerIsDead()
    {
        endState = true;
    }
}