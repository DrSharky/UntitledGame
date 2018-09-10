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
            if(collision.collider.tag == "Monster" || collision.collider.tag == "Chill")
            {
                EventManager.TriggerEvent("explode" + collision.collider.gameObject.GetInstanceID());
            }
        }
        Destroy(gameObject);
    }
}