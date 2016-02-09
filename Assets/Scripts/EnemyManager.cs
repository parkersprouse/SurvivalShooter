using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour {

    [System.Serializable]
    public class Wave {

        public string name; // Name of the wave, can be used for identification
        public Transform enemyPrefab; // The enemy we will be spawning
        public int amount; // Number of enemies to spawn this wave
        public float spawnRate; // How quickly enemies are spawned in this wave

    }

    public enum SpawnState {
        SPAWNING,
        WAITING,
        COUNTING
    }
    private SpawnState spawnState;

    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5; // Time in seconds between wave spawns

    private int nextWaveIndex = 0;
    private float countdownToNextWave;
    private float checkForEnemyRate = 1;

    void Start() {
        countdownToNextWave = timeBetweenWaves;
        spawnState = SpawnState.COUNTING;
    }

    void Update() {
        if (spawnState == SpawnState.WAITING) {
            if (WaveCleared()) {
                WaveCompleted();
            }
            return;
        }

        if (countdownToNextWave <= 0) {
            if (spawnState != SpawnState.SPAWNING) {
                StartCoroutine(SpawnWave(waves[nextWaveIndex]));
            }
        }
        else {
            countdownToNextWave -= Time.deltaTime;
        }
    }

    private void WaveCompleted() {
        spawnState = SpawnState.COUNTING;
        countdownToNextWave = timeBetweenWaves;
        nextWaveIndex++;

        if (nextWaveIndex >= waves.Length) {
            nextWaveIndex = 0;
            Debug.Log("restarting");
        }
    }

    private bool WaveCleared() {
        checkForEnemyRate -= Time.deltaTime;
        if (checkForEnemyRate <= 0) {
            checkForEnemyRate = 1;
            return GameObject.FindGameObjectWithTag("Enemy") == null;
        }
        return false;
    }
    
    private void SpawnEnemy(Transform enemy) {
        Transform s = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, s.position, s.rotation);
    }


    IEnumerator SpawnWave(Wave w) {
        Debug.Log("Spawning Wave: " + w.name);
        spawnState = SpawnState.SPAWNING;

        for (int i = 0; i < w.amount; i++) {
            SpawnEnemy(w.enemyPrefab);
            yield return new WaitForSeconds (1f / w.spawnRate); 
        }

        spawnState = SpawnState.WAITING;
        yield break;
    }

}
