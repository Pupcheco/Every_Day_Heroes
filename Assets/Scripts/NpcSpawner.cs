using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class NpcSpawner : MonoBehaviour {

    const int _spawnQueueCount = 20;

    [SerializeField, ReorderableList] List<GameObject> _npcPrefabs = new List<GameObject>();

    Queue<GameObject> _npcQueue = new Queue<GameObject>();

    public void SpawnNpc(int number) {
        for (var i = 0; i < number; ++i) {
            Vector3 spawnLocation = new Vector3();
            for (var j = 0; j < 30; ++j) {
                var vec2 = Vector2.zero + (Random.insideUnitCircle * 30f);
                var randomPoint = new Vector3(vec2.x, 0f, vec2.y);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                    spawnLocation = hit.position;
                    break;
                }
            }
            var npc = _npcQueue.Dequeue();
            npc.transform.position = spawnLocation;
            npc.SetActive(true);
        }
    }

    public void Enqueue(GameObject npc) {
        _npcQueue.Enqueue(npc);
    }

    void Start() {
        for (var i = 0; i < _spawnQueueCount; ++i) {
            var index = Random.Range(0, _npcPrefabs.Count);
            var npc = Instantiate(_npcPrefabs[index], Vector3.zero, Quaternion.identity, this.transform);
            npc.SetActive(false);
            _npcQueue.Enqueue(npc);
        }

        SpawnNpc(10);
    }
}
