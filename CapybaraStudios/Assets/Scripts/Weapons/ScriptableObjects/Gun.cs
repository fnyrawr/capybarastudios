using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName ="Gun")]
public class Gun : ScriptableObject
{
    public new string name;
    public GameObject weaponPrefab;
    public int dmg;
    public int ammo;
    public int range;
    public int spray;
    public int cooldown;
}
