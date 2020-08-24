﻿#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
using Ludiq;

public class Superheroes : MonoBehaviour {
    
    [SerializeField] Transform _target;
    [SerializeField] float _retargetStrength = 1000f;
    [SerializeField] GameObject _explosion;
    Rigidbody _rigidbody;

    void OnEnable() {
        _rigidbody = this.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision) {
        var normal = collision.GetContact(0).normal;
        if (collision.gameObject.layer == 11) {
            CustomEvent.Trigger(_explosion, "Set Visible");
            _rigidbody.AddForce(normal * 100f, ForceMode.Impulse);
        } else if (collision.gameObject.layer == 31) {
            _rigidbody.AddForce(new Vector3(0f, -5f, 0f), ForceMode.Impulse);
        }
        if (_rigidbody.velocity.sqrMagnitude < 2f) {
            normal.y += 0.5f;
            _rigidbody.AddForce(normal * 10f, ForceMode.Impulse);
        }
    }

    void Update() {
        var force = _target.position - this.transform.position;
        var distance = Vector3.Distance(_target.position, this.transform.position);
        var distanceFactor = 1 / distance;
        _rigidbody.AddForce(force * Time.deltaTime * distanceFactor * _retargetStrength);
    }
}
