#pragma warning disable 649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {
    
    public enum NPCState {
        Idle,
        Following,
    }

    [SerializeField] NPCData _npc;

    GameLogic _gameLogic;
    NavMeshAgent _agent;
    NPCState _state = NPCState.Idle;
    Transform _followTarget;
    Transform _wanderTarget;
    float _wanderTime = -10f;
    float _timeBetweenRepaths = 0.5f;
    float _repathTime = -10f;

    void OnEnable() {
        _gameLogic = GameObject.Find("Game Logic").GetComponent<GameLogic>();
        _agent = this.GetComponent<NavMeshAgent>();
        _followTarget = GameObject.Find("FollowTarget").transform;
    }

    void Start() {
        // Create new WanderTarget for each agent, with the Game Logic object as its parent.
        var thisName = this.name + " WanderTarget";
        var target = new GameObject(thisName);
        target.transform.parent = _gameLogic.transform;
        _wanderTarget = target.transform;
        _agent.speed = _npc.WanderSpeed;
    }

    void Update() {
        // Wander if idle.
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

        if (_state == NPCState.Following && Time.time > _repathTime) {
            _agent.SetDestination(_followTarget.position);
            _repathTime = Time.time + _timeBetweenRepaths;
        }
    }

    public void FollowPlayer() {
        _agent.SetDestination(_followTarget.position);
        _state = NPCState.Following;
        _agent.speed = _npc.FollowSpeed;
        _repathTime = Time.time + _timeBetweenRepaths;
        ++_gameLogic.FollowerCount;
    }
}
