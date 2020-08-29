using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {
    
    [SerializeField] float _timeBetweenDamage = 0.5f;
    [SerializeField] float _impactToFollowerLossRatio = 0.01f;
    [SerializeField] float _minimumImpact = 30f;
    float _nextDamageTime = -10f;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Debris") && Time.time > _nextDamageTime &&
            collision.impulse.magnitude > _minimumImpact && FollowerManager.Followers.Count > 0) {
            var count = (int)(collision.impulse.magnitude * _impactToFollowerLossRatio);
            FollowerManager.LoseFollowers(count);
            _nextDamageTime = Time.time + _timeBetweenDamage;
        }
        if (FollowerManager.Followers.Count == 0) {
            GameObject.Find("Game Logic").GetComponent<GameLogic>().GameOver();
        }
    }
}
