using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerup;
    [SerializeField]
    private GameObject[] _commonPowerups;
    [SerializeField]
    private GameObject[] _collectibles;
    private int _randomEnemy;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnCommonPowerupRoutine());
        StartCoroutine(SpawnCollectiblesRoutine());
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3f);
        while (_stopSpawning == false)
        {
            _randomEnemy = Random.Range(0, _enemyPrefab.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            GameObject enemy = Instantiate(_enemyPrefab[_randomEnemy], spawnPos, _enemyPrefab[_randomEnemy].transform.rotation);
            enemy.transform.parent = _enemyContainer.transform;
            float randomSpawn = Random.Range(2f, 5f);
            yield return new WaitForSeconds(randomSpawn);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(6f);
        while (_stopSpawning == false)
        {
            int randomIndex = Random.Range(0, _powerup.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            Instantiate(_powerup[randomIndex], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }
    IEnumerator SpawnCommonPowerupRoutine() //Guaranteed ammo spawn every 10 seconds
    {
        yield return new WaitForSeconds(3f);
        while(_stopSpawning == false)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            Instantiate(_commonPowerups[0], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }
    IEnumerator SpawnCollectiblesRoutine()
    {
        yield return new WaitForSeconds(Random.Range(20, 25));
        while(_stopSpawning == false)
        {
            int randomIndex = Random.Range(0, _collectibles.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);
            Instantiate(_collectibles[randomIndex], spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(25, 40));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
