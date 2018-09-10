using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text speedText;
    public GameObject levelEndObject;
    public GameObject gameOverObject;

    public string[] skeletonText = new string[4];

    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    public GameObject dialogueText;
    [SerializeField]
    private GameObject playerHealth;

    private string dialogueType;
    private int dialogueIndex = 0;
    private int speedMulti = 0;

    private Rigidbody playerRb;
    private Transform playerCam;
    private UnityEvent sunglassesEvent;

    public static bool sunglasses = false;
    public static bool initSun = false;

    public static void sunglassesEventFired()
    {
        sunglasses = true;
        initSun = true;
    }

	// Use this for initialization
	void Start()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        if (sunglassesEvent == null)
            sunglassesEvent = new UnityEvent();
        sunglassesEvent.AddListener(sunglassesEventFired);
	}

    public void DisplayDialoguePanel(string type)
    {
        dialogueType = type;
        playerHealth.SetActive(false);
        dialoguePanel.SetActive(true);
    }

    private void Update()
    {
        if(dialogueText.activeInHierarchy)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition;
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (dialogueIndex < skeletonText.Length)
                {
                    if (dialogueType == "ChillSkeleton")
                    {
                        NextDialogueLine(skeletonText[dialogueIndex]);
                        dialogueIndex++;

                        if(dialogueIndex == 4)
                        {
                            sunglassesEvent.Invoke();
                        }

                    }
                    else if (dialogueType == "RatKing")
                    {
                        //TO-DO
                    }
                }
                else
                {
                    ClearDialoguePanel();
                    dialogueIndex = 0;
                    playerRb.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        }
    }

    void NextDialogueLine(string nextLine)
    {
        dialogueText.GetComponent<Text>().text = nextLine;
    }

    void ClearDialoguePanel()
    {
        playerHealth.SetActive(true);
        dialoguePanel.SetActive(false);
    }

    public void OnSpeedPickup()
    {
        speedMulti += 1;
        speedText.text = "X " + speedMulti;
    }

    public void OnLevelEnd()
    {
        levelEndObject.SetActive(true);
    }

    public void OnGameOver()
    {
        gameOverObject.SetActive(true);
    }
}