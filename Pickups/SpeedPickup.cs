using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedPickup : MonoBehaviour
{
    public UnityEvent playerCollide;
    public static UIManager uiManager;

    RigidbodyFirstPersonController rigidBodyScript;

	// Use this for initialization
	void Start()
	{
        rigidBodyScript = GameObject.FindGameObjectWithTag("Player").GetComponent<RigidbodyFirstPersonController>();
        uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        playerCollide.AddListener(rigidBodyScript.IncreaseSpeed);
        playerCollide.AddListener(uiManager.OnSpeedPickup);
        StartCoroutine(SpinPickup());
	}

    IEnumerator SpinPickup()
    {
        while (gameObject != null)
        {
            transform.Rotate(0.0f, 1.7f, 0.0f, Space.World);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCollide.Invoke();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
	{
		
	}
}