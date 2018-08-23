using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text speedText;
    public GameObject[] levelEndObjects = new GameObject[5];
    public GameObject[] gameOverObjects = new GameObject[5];

    private int speedMulti = 0;

	// Use this for initialization
	void Start()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

    public void OnSpeedPickup()
    {
        speedMulti += 1;
        speedText.text = "X " + speedMulti;
    }

    public void OnLevelEnd()
    {
        for(int i = 0; i < levelEndObjects.Length; i++)
        {
            if(levelEndObjects[i] != null)
                levelEndObjects[i].SetActive(true);
        }
    }

    public void OnGameOver()
    {
        for(int i = 0; i < gameOverObjects.Length; i++)
        {
            if (gameOverObjects[i] != null)
                gameOverObjects[i].SetActive(true);
        }
    }
}