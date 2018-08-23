using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            GameManager.LevelEnd.Invoke();
    }

}