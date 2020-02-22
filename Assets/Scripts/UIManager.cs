using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesDisplay;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _noAmmoText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _congratulationsText;
    [SerializeField]
    private Text _defeatedBossText;
    [SerializeField]
    private Sprite[] _bossHealthBarSprites;
    [SerializeField]
    private Image _bossHealthBarDisplay;

    [SerializeField]
    private Text _waveText;
    private bool _noAmmo = false;
    void Start()
    {
        _ammoText.text = "Ammo: " + 0;
        _scoreText.text = "Score: " + 0;
        _livesDisplay.sprite = _livesSprites[3];
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _noAmmoText.enabled = false;
        _defeatedBossText.enabled = false;
        _congratulationsText.enabled = false;
        _waveText.enabled = false;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _livesSprites[currentLives];
    }

    public void UpdateWave(int currentWave)
    {
        int showWave = currentWave + 1;
        StartCoroutine(WaveRoutine(showWave));
    }

    IEnumerator WaveRoutine(int currentWave)
    {
        _waveText.text = "Wave " + currentWave;
        _waveText.enabled = true;
        yield return new WaitForSeconds(4f);
        _waveText.enabled = false;
    }
    public void UpdateBossHealth(int currentHealth)
    {
        _bossHealthBarDisplay.sprite = _bossHealthBarSprites[currentHealth];
    }
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
        if(currentAmmo == 0)
        {
            _noAmmo = true;
        }
        else
        {
            _noAmmo = false;
        }
    }

    public void GameOverSequence()
    {
        _restartText.enabled = true;
        StartCoroutine(GameOverFlickerEffect());
    }

    public void VictorySequence()
    {
        _defeatedBossText.enabled = true;
        _restartText.enabled = true;
        StartCoroutine(CongratulationsFlickerEffect());
    }

    IEnumerator CongratulationsFlickerEffect()
    {
        while(true)
        {
            _congratulationsText.enabled = true;
            yield return new WaitForSeconds(1f);
            _congratulationsText.enabled = false;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator GameOverFlickerEffect()
    {
        while (true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void NoAmmoSequence()
    {
        StartCoroutine(NoAmmoFlickerEffect());
    }

    IEnumerator NoAmmoFlickerEffect()
    {
        while (_noAmmo == true)
        {
            _noAmmoText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _noAmmoText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
