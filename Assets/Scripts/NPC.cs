#pragma warning disable 649
#pragma warning disable 414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;

public class NPC : MonoBehaviour {
    
    public enum NPCState {
        Idle,
        Following,
        Dying,
    }


    [SerializeField] NPCData _npc;

    NavMeshAgent _agent;
    Rigidbody _rigidbody;
    NPCState _state = NPCState.Idle;
    int _id;
    Transform _followTarget;
    Transform _wanderTarget;
    float _wanderTime = -10f;
    float _repathTime = -10f;
    float _refollowTime = -10f;

    public void FollowPlayer() {
        _state = NPCState.Following;
        _agent.speed = _npc.FollowSpeed;
        _repathTime = Time.time + _npc.RepathInterval;

        _id = GameLogic.Followers.Count;
        GameLogic.Followers.Add(this);

        if (_npc.GameLogic.Snaking && _id > 0) {
            _followTarget = GameLogic.Followers[_id - 1].transform;
        }
        _agent.SetDestination(_followTarget.position);
    }

    public void BackUp(Vector3 direction) {
        _rigidbody.MovePosition(direction);
        _refollowTime = Time.time + 1.5f;
    }

    void OnEnable() {
        _agent = this.GetComponent<NavMeshAgent>();
        _rigidbody = this.GetComponent<Rigidbody>();

        var transforms = GameObject.FindWithTag("Player").GetComponentsInChildren<Transform>();
        foreach (var transform in transforms) {
            if (transform.name == "FollowTarget") {
                _followTarget = transform;
                break;
            }
        }

        _agent.acceleration = _npc.Acceleration;
        _agent.stoppingDistance = _npc.StoppingDistance;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 10 && _state != NPCState.Dying && 
            collision.impulse.magnitude > _npc.MinImpulseToKill) {
            _state = NPCState.Dying;
            _agent.enabled = false;
            _rigidbody.isKinematic = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            _rigidbody.AddForce(collision.impulse, ForceMode.Impulse);
        }
    }

    void Start() {
        // Create new WanderTarget for each agent, with the Game Logic object as its parent.
        var thisName = this.name + " WanderTarget";
        var target = new GameObject(thisName);
        GameObject targets;
        if (GameObject.Find("WanderTargets") == null) {
            targets = new GameObject("WanderTargets");
        } else {
            targets = GameObject.Find("WanderTargets");
        }
        target.transform.parent = targets.transform;
        _wanderTarget = target.transform;
        _agent.speed = _npc.WanderSpeed;
    }

    void Update() {
        // Wander if idle.
        if (_agent.enabled) {
            if (!_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance || !_agent.hasPath) &&
                _state == NPCState.Idle && Time.time > _wanderTime) {
                
                // Find a random point on the NavMesh within the WanderDistance range to wander to.
                for (var i = 0; i < 30; ++i) {
                    Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * _npc.WanderDistance;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                        _wanderTarget.position = hit.position;
                        break;
                    }
                }
                _agent.destination = _wanderTarget.position;
                _wanderTime = Time.time + _npc.TimeBetweenWanderings;
            }

            // Make sure the NPC regularly repaths towards the player.
            if (_state == NPCState.Following && Time.time > _repathTime && Time.time > _refollowTime) {
                _agent.SetDestination(_followTarget.position);
                _repathTime = Time.time + _npc.RepathInterval;
            }
        }
    }
}
