using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FollowerManager : MonoBehaviour {

    public static List<NPC> Followers = new List<NPC>();

    [InfoBox("Are the NPCs snaking behind the player in a line? If not, they group behind the player instead.")]
    public bool Snaking = false; 

    public void LoseFollowers(int count) {
        for (var i = 0; i < count; ++i) {
            var index = Random.Range(0, Followers.Count);
            Followers[index].Damage();
        }
    }

    void Start() {
        Followers.Clear();
    }
}
