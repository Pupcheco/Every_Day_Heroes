using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTrigger : MonoBehaviour {
    
    NPC _npc;
    bool _following = false;

    void OnEnable() {
        _npc = this.GetComponentInParent<NPC>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (!_following) {
                _npc.FollowPlayer();
                _following = true;
            } else {
                var playerPosition = other.transform.position;
                var direction = playerPosition - this.transform.position;
                direction = this.transform.position - direction;
                _npc.BackUp(direction);
            }
        }
    }
}
