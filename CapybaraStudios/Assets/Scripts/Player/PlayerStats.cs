using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 100;
    private int damageTaken = 0;
    public bool isDummy;

    public TextMeshPro damageText;
    public TextMeshPro totalDamageText;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0) return;
        Debug.Log("Take Damage: " + damageAmount);
        //take damage
        playerHealth -= damageAmount;
        damageTaken += damageAmount;
        if (isDummy)
        {
            damageText.text = damageAmount.ToString();
            int newTotalDamage;
            Int32.TryParse(totalDamageText.text, out newTotalDamage);
            newTotalDamage += damageAmount;
            totalDamageText.text = newTotalDamage.ToString();
        }
        //die if health is < 0
        if (playerHealth <= 0 && !isDummy)
        {
            //_animator.SetLayerWeight(1,0);
            _animator.applyRootMotion = true;
            _animator.SetTrigger("dying");
            _animator.SetTrigger("dying2");
            //TODO MORE!!!
        }
    }
}