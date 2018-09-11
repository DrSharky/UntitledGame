using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sunglasses : MonoBehaviour
{
    Action sunglassesListener;
    private bool acquired = false;
    private GameObject childObj;

     void Awake()
    {
        sunglassesListener = new Action(Acquire);
        EventManager.StartListening("Sunglasses", sunglassesListener);
    }
    // Use this for initialization
    void Start()
	{
        childObj = transform.GetChild(0).gameObject;
	}

    public void Acquire()
    {
        childObj.SetActive(true);
        acquired = true;
        EventManager.TriggerEvent("SunglassesToggle");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && acquired)
        {
            if (childObj.activeInHierarchy)
                childObj.SetActive(false);
            else
                childObj.SetActive(true);
            EventManager.TriggerEvent("SunglassesToggle");
        }
    }
}