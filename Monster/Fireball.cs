using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fireball : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;

    private PlayerHealth playerHealth;
    private UnityEvent explodeSkeleton;


	// Use this for initialization
	void Start()
	{
        rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
	}
	

    private void OnCollisionEnter(Collision collision)
    {
        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        if (collision.collider.tag == "Player")
        {
            playerHealth.TakeDamage(5);
        }
        else
        {
            if(collision.collider.tag == "Monster")
            {
                explodeSkeleton = new UnityEvent();
                explodeSkeleton.AddListener(collision.collider.GetComponentInChildren<SkelExplode>().Boom);
                explodeSkeleton.Invoke();
            }
            //else if(collision.collider.tag == "Skele")
            //{
            //    explodeSkeleton = new UnityEvent();
            //    if(collision.collider.name == "Spine3")
            //        explodeSkeleton.AddListener(collision.collider.GetComponent<SkelExplode>().Boom);
            //    else
            //        explodeSkeleton.AddListener(collision.collider.GetComponentInParent<SkelExplode>().Boom);
            //    explodeSkeleton.Invoke();
            //}
        }
        Destroy(gameObject);
    }
}