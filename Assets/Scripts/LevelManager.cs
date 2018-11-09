using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    [HideInInspector] public GameState gameState = GameState.Ready;
    public ViewBase startView;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
    public EnemySpawner enemySpawner;
    public float spawnDuration = 5f;
    public int maxZombies = 10;
    public int zombieSpawned = 0;
    public int powerupsSpawned = 0;
    public int maxpowerups = 3;
    public Transform[] powerupSpawnPoints;
    public PowerUpSpawner powerupSpawner;


    private IEnumerator coSpawnEnemies;
    // Use this for initialization


    void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        coSpawnEnemies = CoSpawnEnemies();
        StartCoroutine(CoSpawnEnemies());
        StartCoroutine(CoSpawnPowerUps());
        gameState = GameState.Playing;
    }

    public void GameOver()
    {
        StopCoroutine(coSpawnEnemies);
        gameState = GameState.GameOver;
        
    }

    public void RestartGame()
    {
        Enemy[] zombies = GameObject.FindObjectsOfType<Enemy>();

        for(int i = 0; i < zombies.Length; i++)
        {
            Destroy(zombies[i].gameObject);
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.Ready;
        startView.Show();
    }
	IEnumerator CoSpawnEnemies()
    {
        yield return new WaitForSeconds(4);
        while (true)
        {
            for(int i = 0; i < enemySpawnPoints.Length; i++)
            {
                if (zombieSpawned >= maxZombies)
                    continue;
                GameObject enemyObj = enemySpawner.SpawnAt(enemySpawnPoints[i].position, enemySpawnPoints[i].rotation);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                enemy.onDead.AddListener(() =>
                {
                    zombieSpawned--;
                });
                zombieSpawned++;
            }
            yield return new WaitForSeconds(spawnDuration);
        }
    }

    IEnumerator CoSpawnPowerUps()
    {
        yield return new WaitForSeconds(4);
        while (true)
        {
            for (int j = 0; j < powerupSpawnPoints.Length; j++)
            {
                if (powerupsSpawned >= maxpowerups)
                    continue;
                GameObject powerup = powerupSpawner.SpawnAt(powerupSpawnPoints[j].position, powerupSpawnPoints[j].rotation);
                PowerUpSpawner power = powerup.GetComponent<PowerUpSpawner>();
                power.onDead.AddListener(() =>
                {
                    powerupsSpawned--;
                });
                powerupsSpawned++;

            }
            yield return new WaitForSeconds(spawnDuration);
        }
    }
}
