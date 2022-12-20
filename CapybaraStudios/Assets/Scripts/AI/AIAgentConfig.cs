using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 1f;
    public float maxDistance = 1f;
    public float maxSightDistance = 5f;
    public float rotationSeconds = 0.5f;
    public float healTime = 1f;
    public int maxHp = 100;
}
