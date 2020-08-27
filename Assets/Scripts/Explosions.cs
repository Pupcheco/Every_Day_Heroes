using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Ludiq;

public class Explosions : MonoBehaviour {
    
    const int _explosionPoolCount = 10;
    [SerializeField] GameObject _explosionPrefab;
    static Queue<GameObject> _explosions = new Queue<GameObject>();

    public static void TriggerExplosion(Vector3 position) {
        var explosion = _explosions.Dequeue();
        explosion.transform.position = position;
        CustomEvent.Trigger(explosion, "Set Visible");
        _explosions.Enqueue(explosion);
    }

    void Start() {
        for (var i = 0; i < _explosionPoolCount; ++i) {
            var explosion = GameObject.Instantiate(_explosionPrefab, this.transform.position, this.transform.rotation, this.transform);
            _explosions.Enqueue(explosion);
        }
    }
}
