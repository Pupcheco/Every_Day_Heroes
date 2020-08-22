using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrigger : MonoBehaviour {
    
    NPC _npc;

    void OnEnable() {
        _npc = this.GetComponentInParent<NPC>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            _npc.FollowPlayer();
        }
    }
}
