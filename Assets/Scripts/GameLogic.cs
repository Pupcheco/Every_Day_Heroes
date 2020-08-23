#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class GameLogic : MonoBehaviour {
    public static List<NPC> Followers = new List<NPC>();

    [InfoBox("Are the NPCs snaking behind the player in a line? If not, they group behind the player instead.")]
    public bool Snaking = false; 

    [BoxGroup("Input")]
    [InfoBox("Make sure these match the exact names from the Input Manager.")]
    [SerializeField] string _pauseButton;

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
    
    void Start() {
        Followers.Clear();
    }

    void Update() {
        if (Input.GetButtonDown(_pauseButton)) {
            Pause();
        }
    }
}
