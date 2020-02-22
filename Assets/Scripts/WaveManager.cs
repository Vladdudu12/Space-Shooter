using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public int EnemiesPerWave;
    public int PowerupsPerWave;
    public GameObject[] _powerups;
    public GameObject _normalEnemy;
    public GameObject _angledEnemyLeft;
    public GameObject _angledEnemyRight;
    public GameObject _smartEnemy;
    public GameObject _rotatingEnemy;
    public GameObject _boss;
    public bool _canSpawnAngledEnemies;
    public bool _canSpawnSmartEnemy;
    public bool _canSpawnRotatingEnemy;
    public bool _canSpawnSpecialPowerups;
    public bool _isBossWave;
}


public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    private int _totalEnemiesInCurrentWave;
    [SerializeField]
    private int _enemiesInWaveLeft;
    private int _spawnedEnemies;
    [SerializeField]
    private int _currentWave;
    [SerializeField]
    private int _totalWaves;


    private int _totalPowerupsInCurrentWave;
    private int _spawnedPowerups;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _blackHole;
    [SerializeField]
    private UIManager _uiManger;
    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentWave = -1;
        _totalWaves = waves.Length - 1;
        //StartNextWave();
    }

    public void StartNextWave()
    {
        _currentWave++;
        _uiManger.UpdateWave(_currentWave);
        if (_currentWave > _totalWaves)
        {
            return;
        }

        _totalEnemiesInCurrentWave = waves[_currentWave].EnemiesPerWave;
        _enemiesInWaveLeft = 0;
        _spawnedEnemies = 0;
        _totalPowerupsInCurrentWave = waves[_currentWave].PowerupsPerWave;
        _spawnedPowerups = 0;

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnEnemies()
    {
        GameObject enemy = waves[_currentWave]._normalEnemy;
        GameObject leftEnemy = waves[_currentWave]._angledEnemyLeft;
        GameObject rightEnemy = waves[_currentWave]._angledEnemyRight;
        GameObject smartEnemy = waves[_currentWave]._smartEnemy;
        GameObject rotatingEnemy = waves[_currentWave]._rotatingEnemy;
        yield return new WaitForSeconds(5f);
        while (_spawnedEnemies < _totalEnemiesInCurrentWave && _stopSpawning == false)
        {
            _spawnedEnemies++;
            _enemiesInWaveLeft++;
            int enemyPicker = Random.Range(0,5);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);

            if (waves[_currentWave]._canSpawnRotatingEnemy == true && enemyPicker == 4)
            {
               GameObject newEnemy = Instantiate(rotatingEnemy, spawnPos, rotatingEnemy.transform.rotation);
               newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (waves[_currentWave]._canSpawnSmartEnemy == true && enemyPicker == 3)
            {
                GameObject newEnemy = Instantiate(smartEnemy, spawnPos, smartEnemy.transform.rotation);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (waves[_currentWave]._canSpawnAngledEnemies == true && enemyPicker == 2)
            {
                GameObject newEnemy = Instantiate(rightEnemy, spawnPos, rightEnemy.transform.rotation);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if(waves[_currentWave]._canSpawnAngledEnemies == true && enemyPicker == 1)
            {
                GameObject newEnemy = Instantiate(leftEnemy, spawnPos, leftEnemy.transform.rotation);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                GameObject newEnemy = Instantiate(enemy, spawnPos, enemy.transform.rotation);
                newEnemy.transform.parent = _enemyContainer.transform;
            }

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
        yield return null;
    }

    IEnumerator SpawnPowerUps()
    {
        GameObject repair = waves[_currentWave]._powerups[6];
        GameObject missile = waves[_currentWave]._powerups[5];
        GameObject ammo = waves[_currentWave]._powerups[4];
        yield return new WaitForSeconds(8f);
        while (_spawnedPowerups < _totalPowerupsInCurrentWave)
        {
            _spawnedPowerups++;
            int powerupPicker = Random.Range(0, 10);
            Vector3 spawnPos = new Vector3(Random.Range(-9.5f, 9.5f), 6f, 0f);

            if (waves[_currentWave]._canSpawnSpecialPowerups == true && powerupPicker == 7)
            {
                Instantiate(repair, spawnPos, Quaternion.identity);
            }
            else if (waves[_currentWave]._canSpawnSpecialPowerups == true && powerupPicker == 4)
            {
                Instantiate(missile, spawnPos, Quaternion.identity);
            }
            else if (powerupPicker == 3 || powerupPicker == 5 || powerupPicker == 9)
            {
                Instantiate(ammo, spawnPos, Quaternion.identity);
            }
            else
            {
                int randomPowerup = Random.Range(0, 4);
                Instantiate(waves[_currentWave]._powerups[randomPowerup], spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(8f);
        }
        yield return null;
    }

    public void EnemyDefeated()
    {
        _enemiesInWaveLeft--;

        if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave && waves[_currentWave]._isBossWave == false)
        {
            _blackHole.GetComponent<BlackHole>().ChangeBackground();
            _blackHole.GetComponent<SpriteRenderer>().enabled = true;
            _blackHole.GetComponent<Collider2D>().enabled = true;
        }
        else if (_enemiesInWaveLeft == 0 && _spawnedEnemies == _totalEnemiesInCurrentWave && waves[_currentWave]._isBossWave == true)
        {
            GameObject boss = waves[_currentWave]._boss;
            Instantiate(boss, new Vector3(0f, 13.5f, 0f), boss.transform.rotation);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
