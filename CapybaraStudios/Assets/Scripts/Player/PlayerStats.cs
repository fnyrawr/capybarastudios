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
    public AudioSource deathSound;
    public AudioSource killSound;
    public int maxHealth = 100;
    private int damageTaken = 0;
    [SerializeField] public int currentHealth = 100;
    public bool isDummy;
    public bool isAI;
    public TextMeshPro damageText;
    public TextMeshPro totalDamageText;
    public TextMeshProUGUI healthIndicator;
    public int damage_done = 0;
    public int kills = 0;
    private Animator _animator;
    public bool dead = false;

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
            //print("distance to shot:" + distance);
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

        currentHealth = maxHealth;
        if (isAI)
        {
            agent = GetComponent<AIAgent>();
            currentHealth = agent.config.maxHp;
        }
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

    public int TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0 || currentHealth <= 0) return 0;
        Debug.Log("Take Damage: " + damageAmount);
        //take damage
        int damage_taken = (currentHealth - damageAmount);
        if (damage_taken < 0)
        {
            damage_taken = 0;
        }

        damage_taken = currentHealth - damage_taken;

        currentHealth -= damageAmount;
        damageTaken += damageAmount;
        if (isDummy)
        {
            damageText.text = damageAmount.ToString();
            int newTotalDamage;
            Int32.TryParse(totalDamageText.text, out newTotalDamage);
            newTotalDamage += damageAmount;
            totalDamageText.text = newTotalDamage.ToString();
            return damage_taken;
        }

        if (isAI)
        {
            if (agent.weapons.HasWeapon() && agent.config.aIBehaviour == AIBehaviour.Passiv)
            {
                agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
            }
        }

        blinkTimer = blinkDuration;
        UpdateHealth();
        return damage_taken;
    }


    public void UpdateScore()
    {
    }

    public void UpdateHealth()
    {
        if (isAI) agent.healthBar.SetHealtBar(currentHealth / (float)maxHealth);
        else
        {
            updateVignette();
            healthIndicator.SetText("+" + currentHealth); //TODO
        }

        print(currentHealth);
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void die()
    {
        if (dead)
        {
            return;
        }

        dead = true;
        if (isAI)
        {
            GameManager.kills += 1;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<AiController>().enabled = false;
            GetComponent<AIAgent>().enabled = false;
            agent.stateMachine.ChangeState(AIStateId.Death);
            Destroy(agent.gameObject, 60f);
            if (GetComponent<DummyRespawn>())
            {
                GetComponent<DummyRespawn>().triggerRespawn();
            }
        }
        else
        {
            GetComponent<InputManager>().enabled = false;
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerLook>().enabled = false;
            GetComponent<cullHead>().die();
            GetComponent<PlayerInteract>().enabled = false;
            GetComponent<PlayerUI>().enabled = false;
            GetComponent<WeaponAnimationController>().enabled = false;
            GetComponent<GunScript>().EjectGun();
            GetComponent<GunScript>().EjectGun();
            FindObjectOfType<HUDcontroller>().Death();
        }

        if (!deathSound.isPlaying && !isAI)
        {
            deathSound.Play();
        }

        if (!killSound.isPlaying && isAI)
        {
            killSound.Play();
        }

        GetComponent<Ragdoll>().EnablePhysics();
    }


    public void Heal(int health)
    {
        StartCoroutine(HealOverTime(health));
        if (!isAI && !isDummy) updateVignette();
    }

    public void updateVignette()
    {
        Vignette vignette;
        if (volume.profile.TryGet(out vignette))
        {
            float percent = 0.55f * Math.Max((1.0f - (currentHealth / (float)maxHealth)), 0f);
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