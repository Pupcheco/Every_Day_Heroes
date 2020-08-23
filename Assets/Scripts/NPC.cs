#pragma warning disable 649
#pragma warning disable 414

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using NaughtyAttributes;

public class NPC : MonoBehaviour {
    
    public enum NpcState {
        Idle,
        Following,
        Dying,
    }


    [SerializeField] NPCData _npc;

    NavMeshAgent _agent;
    Rigidbody _rigidbody;
    NpcState _state = NpcState.Idle;
    int _id;
    Transform _followTarget;
    Transform _wanderTarget;
    float _wanderTime = -10f;
    float _repathTime = -10f;
    float _refollowTime = -10f;

    public void FollowPlayer() {
        _state = NpcState.Following;
        _agent.speed = _npc.FollowSpeed;
        _repathTime = Time.time + _npc.RepathInterval;

        _id = FollowerManager.Followers.Count;
        FollowerManager.Followers.Add(this);

        if (_npc.FollowerManager.Snaking && _id > 0) {
            _followTarget = FollowerManager.Followers[_id - 1].transform;
        }
        _agent.SetDestination(_followTarget.position);
    }

    public void Damage(Vector3 impact) {
        FollowerManager.Followers.Remove(this);
        _state = NpcState.Dying;
        _agent.enabled = false;
        _rigidbody.isKinematic = false;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rigidbody.AddForce(impact, ForceMode.Impulse);
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
        if (collision.gameObject.layer == 10 && _state != NpcState.Dying && 
            collision.impulse.magnitude > _npc.MinImpulseToKill) {

            Damage(collision.impulse);
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
                _state == NpcState.Idle && Time.time > _wanderTime) {
                
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
            if (_state == NpcState.Following && Time.time > _repathTime && Time.time > _refollowTime) {
                _agent.SetDestination(_followTarget.position);
                _repathTime = Time.time + _npc.RepathInterval;
            }
        }
    }
}
