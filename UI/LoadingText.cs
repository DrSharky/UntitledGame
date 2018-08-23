using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public Text loadingText;
    public int secondsToWait = 2;
    private float floatSeconds = 2.0f;
    // Use this for initialization
    void Start()
	{
        loadingText = gameObject.GetComponent<Text>();
        loadingText.text = "Loading...";
	}

    // Update is called once per frame
    void Update()
    {
        floatSeconds -= Time.deltaTime;
        float secondsDiff = secondsToWait - floatSeconds;
        if((int)secondsDiff >= 1)
            secondsToWait -= 1;

        loadingText.text = "Loading Time Left: " + secondsToWait;
        if(secondsToWait <= 0)
        {
            loadingText.text = "";
        }
	}
}