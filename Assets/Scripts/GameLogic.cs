#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class GameLogic : MonoBehaviour {

    [ReadOnly] public int FollowerCount;

    [BoxGroup("Input")]
    [InfoBox("Make sure these match the exact names from the Input Manager.")]
    [SerializeField] string _pauseButton;
    
    void Update() {
        if (Input.GetButtonDown(_pauseButton)) {
            Pause();
        }
    }

    public void Pause() {
        Time.timeScale = 0f;
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }

    public void Resume() {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
    }

    public void Quit() {
        Application.Quit();
    }
}
