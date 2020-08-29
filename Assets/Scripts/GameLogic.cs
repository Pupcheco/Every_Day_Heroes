#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using TMPro;

public class GameLogic : MonoBehaviour {
    
    const string _highScorePrefsString = "HighScore";

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

    [InfoBox("Make sure these match the exact names from the Input Manager.")]
    [BoxGroup("Input"), SerializeField] string _pauseButton;

    int _score = 0;
    int _highScore;

    [ShowNonSerializedField] int GAME_OVERED;

    public void Play() {
        _score = 0;
        /*if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(_startMenu)) {
            SceneManager.UnloadSceneAsync(_startMenu);
        }
        if (SceneManager.GetSceneByName(_mainLevel).isLoaded) {
            SceneManager.UnloadSceneAsync(_mainLevel);
        }
        if (SceneManager.GetSceneByName(_gameOver).isLoaded) {
            SceneManager.UnloadSceneAsync(_gameOver);
        }
        */
        SceneManager.LoadSceneAsync(_mainLevel, LoadSceneMode.Single);
        Time.timeScale = 1f;
        ++GAME_OVERED;
    }

    public void Resume() {
        SceneManager.UnloadSceneAsync(_pauseMenu);
        Time.timeScale = 1f;
    }

    public void GameOver() {
        Time.timeScale = 0.001f;
        SceneManager.LoadScene(_gameOver, LoadSceneMode.Additive);
        if (_score >= _highScore) {
            _highScore = _score;
            PlayerPrefs.SetInt(_highScorePrefsString, _highScore);
        }
        _gameOverScore.text = _score.ToString();
        _gameOverHighScore.text = _highScore.ToString();
    }

    public void Quit() {
        Application.Quit();
    }

    public void AddScore(int count) {
        _score += count;
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void Update() {
        if (GAME_OVERED == 0) {
            ++GAME_OVERED;
        }
        if (Input.GetButtonDown(_pauseButton)) {
            if (SceneManager.GetSceneByName(_pauseMenu).isLoaded) {
                Resume();
            } else if (!SceneManager.GetSceneByName(_gameOver).isLoaded) {
                Pause();
            }
        }
        if (Time.time > 3f && GAME_OVERED == 1) {
            GameOver();
            ++GAME_OVERED;
        }
    }

    void Pause() {
        Time.timeScale = 0f;
        SceneManager.LoadScene(_pauseMenu, LoadSceneMode.Additive);
    }
}
