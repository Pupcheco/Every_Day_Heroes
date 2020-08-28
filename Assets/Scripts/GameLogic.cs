#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class GameLogic : MonoBehaviour {
    
    [BoxGroup("Prefabs"), Required("Follower Manager prefab required."), SerializeField] GameObject _followerManager;
    [BoxGroup("Prefabs"), Required("Explosions prefab required."), SerializeField] GameObject _explosionsPrefab;
    [BoxGroup("Prefabs"), SerializeField] GameObject _uiPrefab;

    [BoxGroup("Scenes"), Scene, SerializeField] string _startMenu;
    [BoxGroup("Scenes"), Scene, SerializeField] string _mainLevel;
    [BoxGroup("Scenes"), Scene, SerializeField] string _pauseMenu;

    [InfoBox("Make sure these match the exact names from the Input Manager.")]
    [BoxGroup("Input"), SerializeField] string _pauseButton;

    public void Play() {
        SceneManager.LoadScene(_mainLevel);
        SceneManager.UnloadSceneAsync(_startMenu);
    }

    public void Resume() {
        SceneManager.UnloadSceneAsync(_pauseMenu);
        Time.timeScale = 1f;
    }

    public void Quit() {
        Application.Quit();
    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
        if (Input.GetButtonDown(_pauseButton)) {
            if (!SceneManager.GetSceneByName(_pauseMenu).isLoaded) {
                Pause();
            } else {
                Resume();
            }
        }
    }

    void Pause() {
        Time.timeScale = 0f;
        SceneManager.LoadScene(_pauseMenu, LoadSceneMode.Additive);
    }
}
