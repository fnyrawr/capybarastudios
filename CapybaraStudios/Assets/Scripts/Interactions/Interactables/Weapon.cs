using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Weapon : Interactable
{
    private Transform _transform;

    //sounds
    public AudioSource gunSound;
    public AudioSource reloadSound;
    public AudioSource pickupSound;

    //Gun stats
    public int damage;

    public float initialSpread,
        maxSpread,
        range,
        reloadTime,
        fireRate,
        timeBetweenShooting,
        distanceModifier,
        damageFalloffStart;

    public int maxAmmo, magazineSize, bulletsPerTap;
    public bool hasAmmo, rapidFireEnabled;

    public int bulletsLeft, bulletsShot;
    bool reloading, readyToShoot;
    private float reloadStatus = 1;

    //hitmarker
    private GameObject _hitmarker;

    //bullet hole
    public GameObject bulletHoleGraphic;

    //sniperHUD
    public GameObject SniperHUD;
    //public GameObject CrossHair;

    //HUD
    private TextMeshProUGUI _ammoText;
    private TextMeshProUGUI _maxAmmoText;

    private WaitForSeconds rapidFireWait;
    private int controllerMask = ~(1 << 15);
    private Animator _animator;
    public int weaponSlot;
    public int animationType;

    [SerializeField] private float spreadIncrease = 0.001f;
    [SerializeField] private float spreadDecrease = 0.01f; //per second
    public float currentSpread;

    private float _inaccuracy = 1f;

    //_inaccuracy for extra ai inaccuracy, player inaccuracy = 0
    public int specialWeaponType;
    // 0 ist für nicht special Weapon
    // 1 ist für Grappling Gun
    // 2 ist für sniper

    //Bullet Trail
    public Transform BulletFirePoint;
    public TrailRenderer BulletTrail;
    public float zoom = 1f;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        bulletsShot = bulletsPerTap;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        message = "Pick up [E]";
    }

    public void init(Animator animatior, Transform transform, TextMeshProUGUI ammoText, TextMeshProUGUI maxAmmoText,
        GameObject hitmarker, float inaccuracy = 1f)
    {
        _animator = animatior;
        _transform = transform;
        _ammoText = ammoText;
        _maxAmmoText = maxAmmoText;
        _hitmarker = hitmarker;
        _inaccuracy = inaccuracy;
    }

    private void Update()
    {
        if (reloadStatus < 1) reloadStatus += Time.deltaTime / reloadTime;
        else if (reloadStatus > 1) reloadStatus = 1;

        if (currentSpread == initialSpread) return;
        if (currentSpread < initialSpread) currentSpread = initialSpread;
        else currentSpread -= Time.deltaTime * spreadDecrease;
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log("Picked up " + gameObject.name);
        player.GetComponent<GunScript>().PickUp(gameObject);
        pickupSound.Play();
    }

    public void Shoot(bool first)
    {
        if (reloading) Debug.Log("Reloading");
        if (bulletsLeft <= 0) Debug.Log("no Bullets left");
        if (!readyToShoot) Debug.Log("weapon on cooldown");
        if (!readyToShoot || reloading || bulletsLeft <= 0) return;

        Debug.Log("Shoot!");
        _animator.SetTrigger("shoot");
        readyToShoot = false;


        Vector3 direction;
        if (!first)
        {
            //Calculate Direction with Spread
            direction = _transform.forward + (Vector3)Random.insideUnitSphere * currentSpread * _inaccuracy;
        }
        else
            direction = _transform.forward;

        //spread increase for each shot
        if (currentSpread < maxSpread)
        {
            currentSpread += spreadIncrease * bulletsPerTap;
            if (currentSpread > maxSpread) currentSpread = maxSpread;
        }


        var ray = new Ray(_transform.position, direction);
        var hit_ = ray.origin + direction * range;
        RaycastHit[] hits;
        hits = Physics.RaycastAll(_transform.position, direction, range, controllerMask).OrderBy(x => x.distance)
            .ToArray();


        RaycastHit hit = new RaycastHit();
        foreach (var hit__ in hits)
        {
            if (hit__.transform.root != transform.root)
            {
                hit = hit__;
                break;
            }
        }


        //hit and damage calc
        if (hit.distance != 0)
        {
            Debug.Log(hit.transform.name);
            hit_ = hit.point;
            GameObject collisionObject = hit.collider.gameObject;

            //trail
            if (hasAmmo)
            {
                TrailRenderer trail = Instantiate(BulletTrail, BulletFirePoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            if (collisionObject.CompareTag("Head") || collisionObject.CompareTag("Body") ||
                collisionObject.CompareTag("Limbs"))
            {
                //does object have stats?
                if (collisionObject.GetComponentInParent(typeof(PlayerStats)))
                {
                    //deal damage

                    //body part multiplier
                    float hitMultiplier = 1;
                    if (collisionObject.CompareTag("Head")) hitMultiplier = 3;
                    if (collisionObject.CompareTag("Limbs")) hitMultiplier = 0.75f;
                    float finalDamage = (int)(damage * hitMultiplier);

                    //distance multiplier 0.1- 0.01
                    Debug.Log("Hit Distance = " + hit.distance);
                    if (damageFalloffStart < hit.distance)
                    {
                        float distance = hit.distance / 10f - damageFalloffStart / 10f;
                        float distanceMultiplier = (1 - distance * distanceModifier);
                        if (distanceMultiplier > 1) distanceMultiplier = 1;
                        if (distanceMultiplier < 0) distanceMultiplier = 0;
                        finalDamage *= distanceMultiplier;
                        Debug.Log("distanceMod = " + distanceMultiplier);
                    }

                    //final damage (rounded int)
                    Debug.Log("final damage dealt = (" + finalDamage + "): " + (int)finalDamage);

                    (collisionObject.GetComponentInParent(typeof(PlayerStats)) as PlayerStats).TakeDamage(
                        (int)finalDamage);

                    //player hit particle TODO

                    //Hitmarker
                    HitShow();
                    Invoke(nameof(HitDisable), 0.2f);
                }
            }
            else if (hit.transform.root.tag != "Player" && hit.transform.root.tag != "Enemy")
            {
                //bullet hole if no player was hit
                GameObject bulletHoleClone = Instantiate(bulletHoleGraphic, hit.point + hit.normal * 0.001f,
                    Quaternion.FromToRotation(Vector3.back, hit.normal));
                //bulletHoleClone.transform.position -= bulletHoleClone.transform.forward / 1000;
                Destroy(bulletHoleClone, 10f);
            }
        }
        else
        {
            Debug.Log("Not hit");
        }

        EventManager.Shot(ray.origin, hit_, transform.root);


        //magazine
        bulletsLeft--;
        bulletsShot--;
        ShowAmmo();
        gunSound.Play();
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            readyToShoot = true;
            Shoot(false);
            return;
        }

        Invoke("ResetShot", timeBetweenShooting);

        bulletsShot = bulletsPerTap;
    }

    //shoot cooldown
    private void ResetShot()
    {
        readyToShoot = true;
    }

    //rapid fire
    public IEnumerator RapidFire()
    {
        var shooter = transform.root;
        if (rapidFireEnabled)
        {
            while (transform.root == shooter)
            {
                Shoot(false);
                yield return rapidFireWait;
            }
        }
        /*else
        {
            Shoot(true);
            yield return null;
        }*/
    }

    //reload
    public void Reload()
    {
        if (bulletsLeft.Equals(magazineSize))
        {
            Debug.Log("Magazine is already full");
            return;
        }

        if (maxAmmo <= 0)
        {
            Debug.Log("No ammo left. Cannot reload");
            return;
        }

        if (reloadStatus < 1)
        {
            return;
        }

        Debug.Log("Reload");
        reloading = true;
        readyToShoot = true;
        Invoke("ReloadFinished", reloadTime);
        reloadStatus = 0;
        reloadSound.Play();
    }

    private void ReloadFinished()
    {
        if ((maxAmmo + bulletsLeft) < magazineSize)
        {
            bulletsLeft = maxAmmo + bulletsLeft;
            maxAmmo = 0;
        }
        else
        {
            maxAmmo -= magazineSize - bulletsLeft;
            bulletsLeft = magazineSize;
        }

        ShowAmmo();
        reloading = false;
    }

    public void ShowAmmo()
    {
        if (!_ammoText || !_maxAmmoText) return;
        if (!hasAmmo)
        {
            _ammoText.SetText("∞");
            _maxAmmoText.SetText("");
        }
        else
        {
            _ammoText.SetText((bulletsLeft / bulletsPerTap) + " / " + (magazineSize / bulletsPerTap));
            if (maxAmmo > 0)
                _maxAmmoText.SetText((maxAmmo / bulletsPerTap).ToString());
            else
                _maxAmmoText.SetText("0");
        }
    }

    //hitmarker show and disable
    public void HitShow()
    {
        if (!_hitmarker) return;
        _hitmarker.SetActive(true);
    }

    public void HitDisable()
    {
        if (!_hitmarker) return;
        _hitmarker.SetActive(false);
    }

    //Trail
    public IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 start = trail.transform.position;

        while (time < 1)
        {
            //interpolate between 2 points
            trail.transform.position = Vector3.Lerp(start, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }

    public void ZoomIn()
    {
        SniperHUD.SetActive(true);
        //CrossHair.SetActive(false);
    }

    public void ZoomOut()
    {
        SniperHUD.SetActive(false);
        //CrossHair.SetActive(true);
    }

    public float getReloadStatus()
    {
        return reloadStatus;
    }
}