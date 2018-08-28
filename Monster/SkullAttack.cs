using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkullAttack : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile;
    public GameObject deathExplosion;
    public GameObject fireballPickup;
    public float shootCooldown = 2.0f;
    public bool attacking = false;

    private Animator anim;
    private PlayerHealth playerHealth;
    private bool inRange = false;
    private bool chasing = false;
    private bool endState = false;
    private float distanceToPlayer = 0.0f;
    private bool cooldownWait = false;
    private NavMeshAgent agent;
    private Vector3 shootDirection;
    private int monsterHealth = 90;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        shootDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        //first check if skull is chasing, then check distance.
        if (chasing && playerHealth.health > 0)
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= 8.0f)
                inRange = true;
            else
            {
                inRange = false;
                attacking = false;
                agent.isStopped = false;
            }

            if (inRange)
                Attack();
        }

        if (playerHealth.health <= 0)
            anim.SetBool("PlayerDead", true);
    }

    private void Attack()
    {
        if (!chasing || endState)
            return;

        attacking = true;
        Rigidbody rb = GetComponent<Rigidbody>();

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        shootDirection = (player.transform.position - transform.position).normalized;
        if (hit.collider == null)
        {
            Quaternion targetRot = Quaternion.LookRotation(shootDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, 3.0f);
            rb.MoveRotation(targetRot);
        }
        else if (hit.collider.tag != "Player" && hit.collider.tag != "Fireball")
        {
            shootDirection = player.transform.position - transform.position.normalized;
            Quaternion targetRot = Quaternion.LookRotation(shootDirection);
            rb.MoveRotation(targetRot);
        }
        else if(hit.collider.tag == "Fireball")
        {
            rb.MoveRotation(transform.rotation);
        }

        else if(!cooldownWait && hit.collider.tag == "Player")
            ShootFireBall(shootDirection);
    }

    private void ShootFireBall(Vector3 shootDir)
    {
        GameObject fireball = Instantiate(projectile, transform.position + (transform.forward), Quaternion.identity);
        fireball.GetComponent<Rigidbody>().velocity = shootDir * 10;
        fireball.transform.right = -shootDir;
        cooldownWait = true;
        StartCoroutine(StartCooldown());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        cooldownWait = false;
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

    public void TakeDamage()
    {
        if(monsterHealth > 0)
            monsterHealth -= 30;
        else
            Death();
    }

    private void Death()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Instantiate(fireballPickup, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}