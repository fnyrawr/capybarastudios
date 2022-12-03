using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 100;

    public TextMeshPro damageText;
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
        //take damage
        playerHealth -= damageAmount;
        //damageText.text = damageAmount.ToString();
        //die if health is < 0
        if (playerHealth <= 0)
        {
            //_animator.SetLayerWeight(1,0);
            _animator.applyRootMotion = true;
            _animator.SetTrigger("dying");
            _animator.SetTrigger("dying2");
            //TODO MORE!!!
        }
    }
}