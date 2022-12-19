using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int damageTaken = 0;
    [SerializeField] private int currentHealth = 100;
    public bool isDummy;
    public bool isAI;
    public TextMeshPro damageText;
    public TextMeshPro totalDamageText;
    public TextMeshProUGUI healthIndicator;
    private Animator _animator;
    //private Ragdoll ragdoll;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Color color;
    [SerializeField] private float blinkDuration, blinkIntensity;
    private float blinkTimer;
    private AIAgent agent;
    [SerializeField] private Volume volume;

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
        if (damageAmount <= 0 || currentHealth <= 0) return;
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
        UpdateHealth();
    }

    void UpdateHealth()
    {
        if (isAI) agent.healthBar.SetHealtBar(currentHealth / (float)maxHealth);
        else {
            updateVignette();
            healthIndicator.SetText("+" + currentHealth); //TODO
        }
        print(currentHealth);
        if (currentHealth <= 0)
        {
            //_animator.SetLayerWeight(1,0);
            //_animator.SetTrigger("dying");
            //_animator.SetTrigger("dying2");
            GetComponent<Ragdoll>().EnablePhysics();
            if (isAI)
            {
                GetComponent<NavMeshAgent>().enabled = false;
                GetComponent<AiController>().enabled = false;
                AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
                agent.stateMachine.ChangeState(AIStateId.Death);
            }
            else {
                //TODO MORE!!!

            }
            //disable other scripts, show death screen, drop all weapons
        }
    }

    public void Heal(int health)
    {
        StartCoroutine(HealOverTime(health));
        updateVignette();
    }

    public void updateVignette() {
        Vignette vignette;
        if(volume.profile.TryGet(out vignette)) {
            float percent = 0.55f * Math.Max((1.0f - (currentHealth / (float) maxHealth)), 1f);
            vignette.intensity.value = percent;
        }
    }

    private IEnumerator HealOverTime(int health)
    {
        if (health > 0 && currentHealth > 0 && currentHealth < maxHealth)
        {
            UpdateHealth();
            currentHealth += 1;
            UpdateHealth();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(HealOverTime(health - 1));
        }
    }
}