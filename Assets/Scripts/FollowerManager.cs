using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FollowerManager : MonoBehaviour {

    public static List<NPC> Followers = new List<NPC>();

    [InfoBox("Are the NPCs snaking behind the player in a line? If not, they group behind the player instead.")]
    public bool Snaking = false;
    [SerializeField] GameLogic _gameLogic;

    public static void LoseFollowers(int count) {
        count = Mathf.Clamp(count, 0, Followers.Count);
        for (var i = 0; i < count; ++i) {
            var index = Random.Range(0, Followers.Count);
            var x = Random.Range(0f, 25f);
            var z = Random.Range(0f, 25f);
            var impact = new Vector3(x, 100f, z);
            Followers[index].Damage(impact);
        }
    }

    public void DropOff() {
        _gameLogic.AddScore(Followers.Count);
        foreach (var follower in Followers) {
            follower.DropOff();
        }
        Followers.Clear();
    }

    void Start() {
        Followers.Clear();
    }
}
