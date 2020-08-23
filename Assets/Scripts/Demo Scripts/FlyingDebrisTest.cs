using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDebrisTest : MonoBehaviour {
    
    void OnEnable() {
        this.GetComponent<Rigidbody>().isKinematic = true;
    }
    void Update() {
        if (Time.time > 60f) {
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f, -2000f), ForceMode.Impulse);
            Debug.Log("shot");
        }
    }
}
