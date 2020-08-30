#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Superheroes : MonoBehaviour {
    
    static float _canExplodeTime = -10f;
    const float _explosionLimitDuration = 0.5f;

    [SerializeField] Transform _target;
    [SerializeField] float _retargetStrength = 1000f;
    [SerializeField] Explosions _explosionsObject;
    Rigidbody _rigidbody;

    void OnEnable() {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision) {
        var normal = collision.GetContact(0).normal;
        if (Time.time > _canExplodeTime) {
            if (collision.gameObject.layer == 11) {
                _canExplodeTime = Time.time + _explosionLimitDuration;
                _explosionsObject.TriggerExplosion(this.transform.position);
                _rigidbody.AddForce(normal * 100f, ForceMode.Impulse);
            } else if (collision.gameObject.layer == LayerMask.NameToLayer("Destructibles") ||
                       collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                _canExplodeTime = Time.time + _explosionLimitDuration;
                _explosionsObject.TriggerExplosion(this.transform.position);
            }
        } else if (collision.gameObject.layer == 31) {
            _rigidbody.AddForce(new Vector3(0f, -5f, 0f), ForceMode.Impulse);
        }
        if (_rigidbody.velocity.sqrMagnitude < 2f) {
            normal.y += 0.5f;
            _rigidbody.AddForce(normal * 10f, ForceMode.Impulse);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) {
            _rigidbody.AddForce(new Vector3(1f, 0f, -1f) * 100f, ForceMode.Impulse);
        }
    }

    void Update() {
        var force = _target.position - this.transform.position;
        var distance = Vector3.Distance(_target.position, this.transform.position);
        var distanceFactor = 1 / distance;
        _rigidbody.AddForce(force * Time.deltaTime * distanceFactor * _retargetStrength);
    }
}
