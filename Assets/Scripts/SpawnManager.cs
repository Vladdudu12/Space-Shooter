using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerup;
    [SerializeField]
    private GameObject[] _collectibles;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnCollectiblesRoutine());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            GameObject enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;

            float randomSpawn = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomSpawn);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            int randomIndex = Random.Range(0, 5);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            Instantiate(_powerup[randomIndex], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    IEnumerator SpawnCollectiblesRoutine()
    {
        yield return new WaitForSeconds(Random.Range(20, 25));
        while(_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            Instantiate(_collectibles[0], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(25, 40));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
