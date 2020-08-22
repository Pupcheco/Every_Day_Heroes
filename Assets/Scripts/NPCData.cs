using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable, CreateAssetMenu(menuName = "Every Day Heroes/NPC")]
public class NPCData : ScriptableObject {

    [MinValue(0f), Tooltip("How quckly the NPC follows behind the player.")] public float FollowSpeed = 8f;
    [MinValue(0f), Tooltip("How quickly the NPC wanders around.")] public float WanderSpeed = 5f;
    [MinValue(0), Tooltip("How far the NPC can wander when not following the player.")] public float WanderDistance = 5f;
    [MinValue(0f), Tooltip("How long the NPC stays still before wandering again.")] public float TimeBetweenWanderings = 1f;


}
