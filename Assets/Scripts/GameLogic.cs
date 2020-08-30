#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using TMPro;

public class GameLogic : MonoBehaviour {
    
    const string _highScorePrefsString = "HighScore";
    
    public int Score = 0;

    [SerializeField] bool _gameOverDebug = false;

    [BoxGroup("Prefabs"), Required("Follower Manager prefab required."), SerializeField] GameObject _followerManager;
    [BoxGroup("Prefabs"), Required("Explosions prefab required."), SerializeField] GameObject _explosionsPrefab;
    [BoxGroup("Prefabs"), SerializeField] GameObject _uiPrefab;

    [BoxGroup("Scenes"), Scene, SerializeField] string _startMenu;
    [BoxGroup("Scenes"), Scene, SerializeField] string _mainLevel;
    [BoxGroup("Scenes"), Scene, SerializeField] string _pauseMenu;
    [BoxGroup("Scenes"), Scene, SerializeField] string _gameOver;

    [BoxGroup("UI"), SerializeField] TextMeshProUGUI _gameOverScore;
    [BoxGroup("UI"), SerializeField] TextMeshProUGUI _gameOverHighScore;
    [BoxGroup("UI"), SerializeField] TextMeshProUGUI _startMenuHighScore;

    [BoxGroup("Input"), SerializeField, InputAxis] string _pauseButton;

    int _highScore;

    [ShowNonSerializedField] int _gameOverTested;

    public void Play() {
        Score = 0;
        SceneManager.LoadSceneAsync(_mainLevel, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public void Resume() {
        SceneManager.UnloadSceneAsync(_pauseMenu);
        Time.timeScale = 1f;
    }

    public void GameOver() {
        Time.timeScale = 0.001f;
        SceneManager.LoadScene(_gameOver, LoadSceneMode.Additive);
        if (Score >= _highScore) {
            _highScore = Score;
            PlayerPrefs.SetInt(_highScorePrefsString, _highScore);
        }
        _gameOverScore.text = Score.ToString();
        _gameOverHighScore.text = _highScore.ToString();
    }

    public void Quit() {
        Application.Quit();
    }

    public void AddScore(int count) {
        Score += count;
    }

    void Awake() {
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Update() {
        if (_gameOverTested == 0 && _gameOverDebug) {
            ++_gameOverTested;
        }
        if (Input.GetButtonDown(_pauseButton)) {
            if (SceneManager.GetSceneByName(_pauseMenu).isLoaded) {
                Resume();
            } else if (!SceneManager.GetSceneByName(_gameOver).isLoaded) {
                Pause();
            }
        }
        if (Time.time > 3f && _gameOverTested == 1 && _gameOverDebug) {
            GameOver();
            ++_gameOverTested;
        }
    }

    void Pause() {
        Time.timeScale = 0f;
        SceneManager.LoadScene(_pauseMenu, LoadSceneMode.Additive);
    }
}
