using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{
    //Player object
    public GameObject player;
    //[FACING VARS] Sight distance = sphere collider size, turnFactor = how fast monster turns to face object.
    public float sightDistance = 20.0f, turnFactor = 2.0f;
    //[CHASING VARS] maxDist = max distance monster will use attack, min distance = min distance monster will use to chase player, moveSpeed = self-explanatory.
    public float maxDist = 10.0f, minDist = 3.0f, moveSpeed = 2.0f;
    //[WANDER VARS] wanderSpeed = speed that monster moves when wandering.
    public float wanderSpeed = 0.1f;
    //Check to see if idle movements should stop.
    private bool inSight = false;
    //Check to see if monster is wandering.
    private bool wandering = false;
    //Check to see if monster is chasing.
    private bool chasing = false;
    //Check to see if monster is in spawn delay freeze mode.
    private bool spawning = true;
    //Coroutine for idle wandering for the monsters.
    private IEnumerator monsterWanderMove;
    //Coroutine for turning for the monsters.
    private IEnumerator monsterWanderTurn;
    //Coroutine for chasing the player.
    private IEnumerator monsterChasePlayer;
    //check if stuck
    private Vector3 lastLocation;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterChasePlayer = ChasePlayer();
        monsterWanderMove = WanderMove();
        monsterWanderTurn = MonsterTurn();
        StartCoroutine(MoveSpawnDelay());
    }

    private IEnumerator MoveSpawnDelay()
    {
        yield return new WaitForSeconds(10.0f);
        spawning = false;
    }

    private void Update()
    {
        if (!spawning)
        {
            if (inSight && !chasing)
            {
                StopCoroutine(monsterWanderMove);
                StopCoroutine(monsterWanderTurn);
                wandering = false;
                monsterChasePlayer = ChasePlayer();
                StartCoroutine(monsterChasePlayer);
                chasing = true;
            }
            else if (!inSight && !wandering)
            {
                wandering = true;
                monsterWanderMove = WanderMove();
                monsterWanderTurn = MonsterTurn();
                StopCoroutine(monsterChasePlayer);
                StartCoroutine(monsterWanderMove);
            }
            if(lastLocation == transform.position)
            {
                StartCoroutine(CheckIfFrozen());
            }
            lastLocation = transform.position;
        }
    }

    private IEnumerator CheckIfFrozen()
    {
        yield return new WaitForSeconds(3.0f);
        if(lastLocation == transform.position)
        {
            StartCoroutine(monsterWanderMove);
        }
    }

    private IEnumerator WanderMove()
    {
        float wanderDistance = Random.Range(6.0f, 20.0f);
        Vector3 origPos = transform.position;
        float distanceMoved = 0.0f;
        while (distanceMoved < wanderDistance && wandering)
        {
            transform.position += transform.forward * wanderSpeed * Time.deltaTime;
            distanceMoved = Vector3.Distance(transform.position, origPos);
            yield return null;
        }
        if(wandering)
            yield return StartCoroutine(monsterWanderTurn);
        wandering = false;
    }

    private IEnumerator MonsterTurn()
    {
        float turnDeg = Random.Range(60.0f, 270.0f);
        Quaternion turnRot = Quaternion.Euler(transform.rotation.eulerAngles.x, turnDeg, transform.rotation.eulerAngles.z);
        float dot = Quaternion.Dot(transform.rotation, turnRot);
        while(dot < 1.0f && wandering)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, turnRot, 0.06f);
            dot = Quaternion.Dot(transform.rotation, turnRot);
            yield return null;
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    StopCoroutine(monsterWanderMove);
    //    if(hit.collider.tag == "Wall")
    //    {
    //        Vector3 newDir = Vector3.Reflect(transform.forward, hit.normal);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StopCoroutine(monsterWanderMove);
            StopCoroutine(monsterWanderTurn);
            wandering = false;
            inSight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            StopCoroutine(monsterChasePlayer);
            inSight = false;
            chasing = false;
        }
    }

    private IEnumerator ChasePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while(inSight)
        {
            agent.SetDestination(player.transform.position);
            //turnTowardsEntity(player);
            //moveTowardsEntity(player);
            yield return null;
        }
    }

    private void turnTowardsEntity(GameObject entity)
    {
        Vector3 direction = entity.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnFactor);
    }

    private void moveTowardsEntity(GameObject entity)
    {
        float distance = Vector3.Distance(transform.position, entity.transform.position);
        if (distance >= minDist)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            if (distance <= maxDist)
            {
                //Possible range attacks go here. (First check to make sure monster is actually facing player decently enough)
            }
        }
    }
}