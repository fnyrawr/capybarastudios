using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int damageTaken = 0;
    private int currentHealth = 100;
    public bool isDummy;
    public bool isAI;
    public TextMeshPro damageText;
    public TextMeshPro totalDamageText;

    private Animator _animator;

    //private Ragdoll ragdoll;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Color color;
    [SerializeField] private float blinkDuration, blinkIntensity;
    private float blinkTimer;
    private AIAgent agent;


    private void OnEnable()
    {
        EventManager.doShoot += Supress;
    }

    private void OnDisable()
    {
        EventManager.doShoot -= Supress;
    }

    void Supress(Vector3 origin, Vector3 hit, Transform player)
    {
        if (player != transform.root)
        {
            Vector3 X;
            if (Vector3.Dot(origin - hit, transform.position - origin) > 0) X = origin;
            else if (Vector3.Dot(hit - origin, transform.position - hit) > 0) X = hit;
            else X = origin + Vector3.Project(transform.position - origin, hit - origin);
            float distance = (X - transform.position + new Vector3(0, 2, 0)).magnitude;
            print("distance to shot:" + distance);
        }
    }


    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        if (!isDummy)
        {
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            color = skinnedMeshRenderers[0].material.color;
        }

        if (isAI)
        {
            agent = GetComponent<AIAgent>();
        }

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDummy) return;
        if (blinkTimer > 0)
        {
            blinkTimer -= Time.deltaTime;
            float intentsity = Mathf.Clamp01(blinkTimer / blinkDuration) * blinkIntensity;
            foreach (var s in skinnedMeshRenderers)
            {
                s.material.color = color + Color.white * intentsity;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (damageAmount < 0 || currentHealth <= 0) return;
        Debug.Log("Take Damage: " + damageAmount);
        //take damage
        currentHealth -= damageAmount;
        damageTaken += damageAmount;
        if (isDummy)
        {
            damageText.text = damageAmount.ToString();
            int newTotalDamage;
            Int32.TryParse(totalDamageText.text, out newTotalDamage);
            newTotalDamage += damageAmount;
            totalDamageText.text = newTotalDamage.ToString();
            return;
        }

        blinkTimer = blinkDuration;
        if (isAI) agent.healthBar.SetHealtBar(currentHealth / (float)maxHealth);
        //die if health is < 0
        if (currentHealth <= 0)
        {
            //_animator.SetLayerWeight(1,0);
            _animator.applyRootMotion = true;
            _animator.SetTrigger("dying");
            _animator.SetTrigger("dying2");
            if (isAI)
            {
                //GetComponent<Ragdoll>().EnablePhysics();
                _animator.applyRootMotion = false;
                AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
                agent.stateMachine.ChangeState(AIStateId.Death);
            }

            //TODO MORE!!!
        }
    }
}