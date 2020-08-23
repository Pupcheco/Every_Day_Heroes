using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable, CreateAssetMenu(menuName = "Every Day Heroes/NPC")]
public class NPCData : ScriptableObject {

    [Required("Game Logic Prefab required.")] public GameLogic GameLogic;

    [Space]
    [MinValue(0f), Tooltip("How often the NPC checks for a new follow position.")] public float RepathInterval = 0.5f;
    [MinValue(0f), Tooltip("How quckly the NPC follows behind the player.")] public float FollowSpeed = 8f;
    [MinValue(0f), Tooltip("How quickly the NPC wanders around.")] public float WanderSpeed = 5f;
    [MinValue(0), Tooltip("How far the NPC can wander when not following the player.")] public float WanderDistance = 5f;
    [MinValue(0f), Tooltip("How long the NPC stays still before wandering again.")] public float TimeBetweenWanderings = 1f;
    [MinValue(0f), Tooltip("How far away from their destination the NPC will stop.")] public float StoppingDistance = 1f;
    [MinValue(0f), Tooltip("How quickly the NPC moves to max speed.")] public float Acceleration = 8f;
    
}
