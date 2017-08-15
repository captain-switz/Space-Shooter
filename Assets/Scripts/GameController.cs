using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    
    /* For spawining hazards */
    
    public GameObject hazard;
    public GameObject fireRateBonusHazard;
    public GameObject fireSpeedBonusHazard;
    public GameObject enemyShip;
    public float bonusOdds;
    public Vector3 spawnValues;
    public int hazardCount;
    public int maxHazardCount;
    public float spawnAcceleration;
    public float minWait;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public float globalSpeed;
    public float speedIncrease;

    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;

    private int score;
    private bool restart;
    private bool gameOver;
    private bool enemySpawned;


    void Start() {

        restart = false;
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";

        score = 0;
        UpdateScore();

        StartCoroutine(SpawnWaves());
    }

    private void Update() {
        if (restart) {
            if (Input.anyKey) {
                Application.LoadLevel(Application.loadedLevel);
            }

        }
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score;
    }

    public void AddScore(int newScore) {
        score = score + newScore;
        UpdateScore();
    }

    public void GameOver() {
        gameOverText.text = "Game Over";
        gameOver = true;
    }
    public bool GetGameOver() {
        return gameOver;
    }

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (true) {
            for (int i = 0; i < hazardCount; i++) {
                if (Random.value < bonusOdds) {
                    if(Random.value < 0.36) {
                        SpawnObject(fireRateBonusHazard);
                    } else {
                        SpawnObject(fireSpeedBonusHazard);
                    }
                } else {
                    SpawnObject(hazard);
                }
                yield return new WaitForSeconds(spawnWait);
            }
            if (spawnWait > minWait) {
                spawnWait = spawnWait - spawnAcceleration;
            }
            if (hazardCount < maxHazardCount) {
                hazardCount++;
            }
            globalSpeed = globalSpeed + speedIncrease;
            
            SpawnObject(enemyShip);
            enemySpawned = true;
            while (enemySpawned) {
                yield return new WaitForSeconds(waveWait);
            }

            if (gameOver) {
                restartText.text = "Tap to restart.";
                restart = true;
                break;
            }
        }
    }

    void SpawnObject(GameObject hazardObject) {
        Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
        Quaternion spawnRotation;
        if (hazardObject.tag == "Enemy") {
            spawnRotation = new Quaternion(0, 180, 0, 0);
        } else {
           spawnRotation = Quaternion.identity;
        }
        Instantiate(hazardObject, spawnPosition, spawnRotation);
    }

    public void EnemyDied() {
        enemySpawned = false;
    }
}
