using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceSpriteTowardsCamera : MonoBehaviour {

    Transform _camera;
    Rigidbody _rigidbody;

    void OnEnable() {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _rigidbody = this.GetComponentInParent<Rigidbody>();
    }

    void Update() {
        if (this.gameObject.layer != 11) {
            this.transform.rotation = _camera.transform.rotation;
        } else {
            var xyVelocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y);
            var angle = Vector2.SignedAngle(Vector2.up, xyVelocity);
            this.transform.rotation = Quaternion.Euler(_camera.transform.rotation.eulerAngles.x, _camera.transform.rotation.eulerAngles.y, angle);
        }
        // = Quaternion.AngleAxis(angle, Vector3.forward);
        //Quaternion.RotateTowards(this.transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 90f);
    }
}
