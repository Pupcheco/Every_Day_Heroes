#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    [SerializeField] GameObject _shatteredPrefab;
    [SerializeField] float _necessaryImpactSize = 10f;
    [FMODUnity.EventRef, SerializeField] string _destructionSound = "";

    void OnCollisionEnter(Collision collision) {
        if (collision.impulse.magnitude > _necessaryImpactSize && collision.gameObject.layer == 11) {
            Object.Instantiate(_shatteredPrefab, this.transform.position, this.transform.rotation);
            Object.Destroy(this.gameObject);

            if (_destructionSound != "") {
                FMODUnity.RuntimeManager.PlayOneShot(_destructionSound, this.transform.position);
            }
        }
    }
}
