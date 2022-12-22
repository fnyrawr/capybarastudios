using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 0.5f;
    public float maxDistance = 20f;
    public float minSightDistance = 20f;
    public float rotationSeconds = 0.5f;
    public float healTime = 1f;
    public float outOfRangeDistance = 35f;
    public int maxHp = 100;
    public float inaccuracy = 5f;
    public float stoppingDistance = 3f;
    public GameObject startWeapon;
}
