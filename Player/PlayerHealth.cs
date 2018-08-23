using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;
    public int fullHealth = 100;
    public GameObject healthUI;

    private Text healthText;
    private bool invoked = false;
    private PlayerAudio playerAudio;

	// Use this for initialization
	void Start()
	{
        healthUI = GameObject.FindGameObjectWithTag("HealthText");
        healthText = healthUI.GetComponent<Text>();
        healthText.text = "Health: " + health;
        playerAudio = GetComponent<PlayerAudio>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(health <= 0)
        {
            health = 0;
            healthText.text = "Health: " + health;

            if (!invoked)
            {
                GameManager.GameOver.Invoke();
                StartCoroutine(playerAudio.PlayDeath());
                invoked = true;
            }
        }
	}

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthText.text = "Health: " + health;
    }
}