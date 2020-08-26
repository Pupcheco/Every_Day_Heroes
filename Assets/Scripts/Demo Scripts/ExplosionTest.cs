using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class ExplosionTest : MonoBehaviour {
    
    [SerializeField] GameObject _explosionObject;
    bool _exploded = false;

    void Update() {
        if (Time.time > 3f && !_exploded) {
            CustomEvent.Trigger(_explosionObject, "Set Visible");
            _exploded = true;
        }
    }
}
