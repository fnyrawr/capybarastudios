using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationState : MonoBehaviour
{
    public Animator animator;

    private string weaponType;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void changeWeapon(string newWeapon)
    {
        if (newWeapon == weaponType) return;
        var newW = animator.GetLayerIndex(newWeapon);
        var oldW = animator.GetLayerIndex(weaponType);
        animator.SetLayerWeight(newW, 1);
        animator.SetLayerWeight(oldW, 0);
        weaponType = newWeapon;
    }
}