#pragma warning disable 649
#pragma warning disable 414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class DustClouds : MonoBehaviour { 
    
    const int _dustCloudPoolCount = 10;
    [SerializeField] GameObject _dustCloudPrefab;
    [FMODUnity.EventRef, SerializeField] string _destructionSound = ""; 
    Queue<GameObject> _dustClouds = new Queue<GameObject>();

    public void TriggerDustCloud(Vector3 position) {
        var cloud = _dustClouds.Dequeue();
        cloud.transform.position = position;
        CustomEvent.Trigger(cloud, "Building collapse");
        _dustClouds.Enqueue(cloud);
    }

    void Start() {
        for (var i = 0; i < _dustCloudPoolCount; ++i) {
            var cloud = GameObject.Instantiate(_dustCloudPrefab, this.transform.position, this.transform.rotation, this.transform);
            _dustClouds.Enqueue(cloud);
        }
    }

}
