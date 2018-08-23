using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public static GameObject player = null;
    public static UIManager uiManager = null;
    public GameObject endLight;

    public static UnityEvent LevelEnd;
    public static UnityEvent GameOver;

    private static int levelCount = 0;

    // Use this for initialization
    void Start()
	{
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if(LevelEnd == null)
            LevelEnd = new UnityEvent();

        if (GameOver == null)
            GameOver = new UnityEvent();

        if (uiManager == null)
            uiManager = GetComponent<UIManager>();

        LevelEnd.AddListener(uiManager.OnLevelEnd);
        LevelEnd.AddListener(AddLevelCount);
        player = GameObject.FindGameObjectWithTag("Player");
        endLight = GameObject.Find("End Light");
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        for(int i = 0; i < monsterObjects.Length; i++)
        {
            WanderingAI aiScript = monsterObjects[i].GetComponent<WanderingAI>();
            MonsterAttack attackScript = monsterObjects[i].GetComponent<MonsterAttack>();
            GameOver.AddListener(aiScript.PlayerIsDead);
            GameOver.AddListener(attackScript.PlayerIsDead);
            LevelEnd.AddListener(aiScript.PlayerIsDead);
            LevelEnd.AddListener(attackScript.PlayerIsDead);
        }
        GameObject[] skullObjects = GameObject.FindGameObjectsWithTag("Skull");
        for(int i=0; i < skullObjects.Length; i++)
        {
            SkullMove aiScript = skullObjects[i].GetComponent<SkullMove>();
            SkullAttack attackScript = skullObjects[i].GetComponent<SkullAttack>();
            GameOver.AddListener(aiScript.PlayerIsDead);
            GameOver.AddListener(attackScript.PlayerIsDead);
            LevelEnd.AddListener(aiScript.PlayerIsDead);
            LevelEnd.AddListener(attackScript.PlayerIsDead);
        }
        GameObject[] ratObjects = GameObject.FindGameObjectsWithTag("Rat");
        for(int i = 0; i < ratObjects.Length; i++)
        {
            RatMove aiScript = ratObjects[i].GetComponent<RatMove>();
            RatAttack attackScript = ratObjects[i].GetComponent<RatAttack>();
            GameOver.AddListener(aiScript.PlayerIsDead);
            GameOver.AddListener(attackScript.PlayerIsDead);
            LevelEnd.AddListener(aiScript.PlayerIsDead);
            LevelEnd.AddListener(attackScript.PlayerIsDead);
        }

        GameOver.AddListener(uiManager.OnGameOver);
        GameOver.AddListener(GetComponent<ScreenFader>().OnGameOver);
    }

    private void AddLevelCount()
    {
        levelCount++;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
	{
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
	}

    //Tied to event from Map.cs. Need to find things after map is loaded.
    //public void LevelStart()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player");
    //    endLight = GameObject.Find("End Light");
    //    GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
    //    for(int i = 0; i < monsterObjects.Length; i++)
    //    {
    //        WanderingAI aiScript = monsterObjects[i].GetComponent<WanderingAI>();
    //        MonsterAttack attackScript = monsterObjects[i].GetComponent<MonsterAttack>();
    //        GameOver.AddListener(aiScript.PlayerIsDead);
    //        GameOver.AddListener(attackScript.PlayerIsDead);
    //        LevelEnd.AddListener(aiScript.PlayerIsDead);
    //        LevelEnd.AddListener(attackScript.PlayerIsDead);
    //    }
    //}
}