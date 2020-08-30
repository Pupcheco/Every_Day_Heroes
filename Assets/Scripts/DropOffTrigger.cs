#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffTrigger : MonoBehaviour {

    [SerializeField] FollowerManager _followers;
    [SerializeField] NpcSpawner _npcSpawner;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            var numberToSpawn = 10 - FollowerManager.Followers.Count;
            _followers.DropOff();
            _npcSpawner.SpawnNpc(numberToSpawn);
        }
    }

}
