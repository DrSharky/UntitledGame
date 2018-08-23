using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthText : MonoBehaviour
{
    public Text healthText;
    public GameObject player;
    private int healthAmt;

	// Use this for initialization
	void Start()
	{
        healthText = gameObject.GetComponent<Text>();

        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        healthAmt = player.GetComponent<PlayerHealth>().health;

        healthText.text = "Health: " + healthAmt;
	}
	
	// Update is called once per frame
	void Update()
	{
        int newHealth = player.GetComponent<PlayerHealth>().health;
		if(healthAmt > newHealth)
        {
            healthAmt = player.GetComponent<PlayerHealth>().health;
            healthText.text = "Health: " + healthAmt;
        }
	}

    public void ChangeHealth()
    {
        healthAmt -= 10;
        healthText.text = "Health: " + healthAmt;

    }
}