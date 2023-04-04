using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    AudioSource levelUpSound;  // possibili: 26 28
    public Player player;
    public bool isInGame;

    [Header("Gameplay elements")]
    public float gameSpeed;
    public float gameTimer;
    public float DistanceToNextLevel;
    public int level;
    public float invincibleTimer = 10;
    public int coinsNum;

    [Header("Spawn elements")]
    public GameObject[] spawnObjects;
    public float spawnTimer;
    public float timeToNextSpawn;
    public float spawnTimerPlus;

    [Header("UI elements")]
    public TMP_Text timerText;
    public TMP_Text levelText;
    public TMP_Text coinsText;
    public TMP_Text snowballText;
    public TMP_Text healthText;
    public GameObject gameoverPanel;
    public TMP_Text timerTextFinal;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in spawnObjects)
        {
            foreach (Transform child in obj.transform)
            {
                child.GetComponent<BoxCollider>().enabled = true;
            }
        }
        /*
        coinsText.text = "Coins: " + player.coins.ToString();
        snowballText.text = "SnowBalls: " + player.snowballsNum.ToString();
        healthText.text = "Health: " + player.health.ToString();
        */

        levelUpSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInGame)
        {
            timerText.text = "Distance: " + Time.timeSinceLevelLoad.ToString();
            timerTextFinal.text = "Distance: " + Time.timeSinceLevelLoad.ToString();

            gameTimer += Time.deltaTime;
            if (gameTimer >= timeToNextSpawn)
            {
                SpawnObject();
                //SpawnWallsTest();
                timeToNextSpawn = Time.timeSinceLevelLoad + spawnTimer + spawnTimerPlus;
                spawnTimerPlus = 0;
            }

            if (gameTimer >= DistanceToNextLevel)
            {
                LevelUp();
                DistanceToNextLevel = gameTimer + 10;
            }
        }      
    }

    private void SpawnObject()
    {
        int rnd = Random.Range(0, spawnObjects.Length);
        GameObject Object = Instantiate(spawnObjects[rnd], gameObject.transform.position, Quaternion.identity);
        Debug.Log(rnd);
        switch(rnd)
        {
            case 0: 
                SpawnPosition(Object);
                break;
            case 1:
                SpawnPosition(Object);
                break;
            case 2:
                SpawnPosition(Object);
                Wall2Lengh(Object);
                break;
            case 3:
                break;
            case 4:
                SpawnPosition(Object);
                Object.transform.position = new Vector3(Object.transform.position.x, 0.5f, Object.transform.position.z);
                break;
            case 5:
                SpawnPosition(Object);
                Object.transform.position = new Vector3(Object.transform.position.x, 0.5f, Object.transform.position.z);
                break;
            case 6:
                SpawnPosition(Object);
                break;
            case 7:
                SpawnPosition(Object);
                break;
            case 8:
                SpawnPosition(Object);
                break;
            case 9:
                SpawnPosition(Object);
                break;
            case 10:
                SpawnPosition(Object);
                Object.transform.position = new Vector3(Object.transform.position.x, 0.5f, Object.transform.position.z);
                Object.transform.eulerAngles = new Vector3(90, Object.transform.eulerAngles.y, Object.transform.eulerAngles.z);
                break;
        }
    }

    public void SpawnPosition(GameObject obj)
    {
        float offsetX = Random.Range(-3, 4);
        obj.transform.position += new Vector3(offsetX, 0, 0);
    }

    public void Wall2Lengh(GameObject obj)
    {
        int lenghZ = Random.Range(1, 7);
        obj.transform.localScale += new Vector3(0, 0, lenghZ);
        spawnTimerPlus = lenghZ / 2;
    }

    public void LevelUp()
    {
        if (spawnTimer > 1.5)
        {
            spawnTimer -= 0.5f;
        }        
        gameSpeed += 3;
        level++;
        levelText.text = level.ToString();
        player.AddSnow();
        levelUpSound.Play();
    }

    private void SpawnWallsTest()
    {
        int rnd = Random.Range(0, spawnObjects.Length);
        Debug.Log(rnd);
        GameObject Object = Instantiate(spawnObjects[rnd], gameObject.transform.position, Quaternion.identity);
        SpawnPosition(Object);

        switch (rnd)
        {
            case 2:
                Wall2Lengh(Object);
                break;
        }
    }

    public void InvinciblePlayer()
    {
        StartCoroutine(InvincibleTimer(invincibleTimer));
    }

    public IEnumerator InvincibleTimer(float invTimer)
    {
        foreach(GameObject obj in spawnObjects)
        {
            foreach(Transform child in obj.transform)
            {
                child.GetComponent<BoxCollider>().enabled = false;             
            }
        }

        yield return new WaitForSeconds(invTimer);

        foreach (GameObject obj in spawnObjects)
        {
            foreach (Transform child in obj.transform)
            {
                child.GetComponent<BoxCollider>().enabled = true;
            }
        }
        yield return null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        timerText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);
        coinsText.gameObject.SetActive(true);
        snowballText.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);

        isInGame = true;
        gameSpeed = 10;
        player.enabled = true;
    }

}
