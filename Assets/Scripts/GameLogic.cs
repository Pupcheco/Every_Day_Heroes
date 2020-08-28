#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class GameLogic : MonoBehaviour {
    

    [Required("Follower Manager prefab required."), SerializeField] GameObject _followerManager;
    [Required("Explosions prefab required."), SerializeField] GameObject _explosionsPrefab;

    [InfoBox("Make sure these match the exact names from the Input Manager.")]
    [BoxGroup("Input"), SerializeField] string _pauseButton;

    public void Resume() {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
    }

    public void Quit() {
        Application.Quit();
    }

    void Start() {
        // Instantiate a Follower Manager object, but only if there isn't one already present in the scene.
        if (GameObject.Find(_followerManager.name) == null) {
            Instantiate(_followerManager, Vector3.zero, Quaternion.identity);
        }
        // Do the same for the Explosions object.
        if (GameObject.Find(_explosionsPrefab.name) == null) {
            Instantiate(_explosionsPrefab, Vector3.zero, Quaternion.identity);
        }
    }

    void Update() {
        if (Input.GetButtonDown(_pauseButton)) {
            if (!SceneManager.GetSceneByName("PauseMenu").isLoaded) {
                Pause();
            } else {
                Resume();
            }
        }
    }

    void Pause() {
        Time.timeScale = 0f;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }
}
