﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class FollowerManager : MonoBehaviour {

    public static List<NPC> Followers = new List<NPC>();

    [InfoBox("Are the NPCs snaking behind the player in a line? If not, they group behind the player instead.")]
    public bool Snaking = false;

    public static void LoseFollowers(int count) {
        for (var i = 0; i < count; ++i) {
            var index = Random.Range(0, Followers.Count);
            var x = Random.Range(0f, 25f);
            var z = Random.Range(0f, 25f);
            var impact = new Vector3(x, 300f, z);
            Followers[index].Damage(impact);
        }
    }

    void Start() {
        Followers.Clear();
    }
}
