using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Events;

public class RatMove : MonoBehaviour
{

    public float wanderRadius;
    public float wanderTimer;
    public GameObject player;

    private NavMeshAgent agent;
    private float timer;
    private bool chasing = false;
    private bool frozen = true;
    private bool endState = false;
    private Rigidbody rb;
    private Animator anim;

    private UnityEvent chaseEvent;
    //private PlayerHealth playerHealth;
    private RatAttack attackScript;

    //Standard Asset adapting stuff
    private float turnAmt;
    private float forwardAmt;
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;

    Vector3 m_GroundNormal;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        //agent.enabled = false;
        StartCoroutine(WaitForSpawn());
    }

    // Use this for initialization
    void OnEnable()
    {
        timer = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        m_GroundNormal = new Vector3(0, 1, 0);

        agent.updateRotation = false;
        agent.updatePosition = true;

        if (chaseEvent == null)
            chaseEvent = new UnityEvent();

        attackScript = GetComponent<RatAttack>();

        chaseEvent.AddListener(attackScript.OnStartChase);
    }

    private IEnumerator WaitForSpawn()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(3.0f);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        agent.enabled = true;
        frozen = false;
    }

    private void FixedUpdate()
    {
        if (frozen || endState)
            return;

        timer += Time.deltaTime;

        if (timer >= wanderTimer && !chasing)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);

            agent.SetDestination(newPos);
            timer = 0;

        }
        else if (chasing)
        {
            agent.SetDestination(player.transform.position - ((player.transform.position - transform.position).normalized * 2));
        }

        if (agent.remainingDistance > agent.stoppingDistance)
            Move(agent.desiredVelocity);
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        anim.SetFloat("Forward", forwardAmt);
        anim.SetFloat("Turn", turnAmt);
    }

    //public void OnAnimatorMove()
    //{
    //    // we implement this function to override the default root motion.
    //    // this allows us to modify the positional speed before it's applied.
    //    Vector3 v = (anim.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

    //    // we preserve the existing y part of the current velocity.
    //    v.y = rb.velocity.y;
    //    rb.velocity = v;
    //}

    public void Move(Vector3 move)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        turnAmt = Mathf.Atan2(move.x, move.z);
        if (chasing)
            forwardAmt = move.z;
        else
            forwardAmt = move.z * 0.5f;

        ApplyExtraTurnRotation();

        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }


    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, forwardAmt);
        transform.Rotate(0, turnAmt * turnSpeed * Time.deltaTime, 0);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chasing = true;
            chaseEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            chasing = false;
    }

    public void PlayerIsDead()
    {
        chasing = false;
        endState = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Color debugColor;
        if (collision.collider.tag == "Monster")
        {
            debugColor = Color.blue;
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, debugColor, 5);
            }
            rb.velocity = new Vector3(0, 0, 0);
        }
        if (collision.collider.tag == "Player")
        {
            debugColor = Color.green;
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, debugColor, 5);
            }
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
}