using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private AudioSource explosionSfx;
    
	// Use this for initialization
	void Start()
	{
        explosionSfx = GetComponent<AudioSource>();
        StartCoroutine(WaitForDeath());
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(explosionSfx.clip.length);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        {
            PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(10);
        }
    }
}