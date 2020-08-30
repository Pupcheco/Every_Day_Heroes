#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Destructible : MonoBehaviour {
    [SerializeField] GameObject _shatteredPrefab;
    [SerializeField] float _necessaryImpactSize = 10f;
    [FMODUnity.EventRef, SerializeField] string _destructionSound = "";
    DustClouds _dustCloudsObject;

    void OnCollisionEnter(Collision collision) {
        if (collision.impulse.magnitude > _necessaryImpactSize && collision.gameObject.layer == 11) {
            var shatteredObject = Object.Instantiate(_shatteredPrefab, this.transform.position, this.transform.rotation);
            shatteredObject.layer = LayerMask.NameToLayer("Debris");
            foreach (var shard in shatteredObject.GetComponentsInChildren<Rigidbody>()) {
                shard.AddForce(collision.impulse, ForceMode.Impulse);
            }

            var dustCloudPosition = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
            _dustCloudsObject.TriggerDustCloud(dustCloudPosition);

            if (_destructionSound != "") {
                FMODUnity.RuntimeManager.PlayOneShot(_destructionSound, this.transform.position);
            }
            
            Object.Destroy(this.gameObject);
        }
    }

    void OnEnable() {
        this.gameObject.layer = LayerMask.NameToLayer("Destructibles");
        _dustCloudsObject = GameObject.Find("Dust Clouds").GetComponent<DustClouds>();
    }
}
