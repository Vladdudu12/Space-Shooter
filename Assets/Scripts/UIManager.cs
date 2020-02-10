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
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _livesDisplay.sprite = _livesSprites[3];
        _gameOverText.enabled = false;
        _restartText.enabled = false;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesDisplay.sprite = _livesSprites[currentLives];
    }

    public void GameOverSequence()
    {
        _restartText.enabled = true;
        StartCoroutine(FlickerEffect());
    }

    IEnumerator FlickerEffect()
    {
        while (true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
