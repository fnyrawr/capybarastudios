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

    public bool singleReload;
    private float singleReloadTime;

    public int maxAmmo, magazineSize, bulletsPerTap;
    public bool hasAmmo, rapidFireEnabled;
    [HideInInspector] public int bulletsLeft, bulletsShot;
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
    // 3 ist für rocket launcher

    //Bullet Trail
    public Transform BulletFirePoint;
    public TrailRenderer BulletTrail;
    public float zoom = 1f;
    private bool _ai;

    private void Awake()
    {
        singleReloadTime = reloadTime / ((float)magazineSize / bulletsPerTap);
        bulletsLeft = magazineSize;
        readyToShoot = true;
        bulletsShot = bulletsPerTap;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        message = "Pick up [E]";
    }

    public void init(Animator animatior, Transform transform, TextMeshProUGUI ammoText, TextMeshProUGUI maxAmmoText,
        GameObject hitmarker, float inaccuracy = 1f, bool ai = false)
    {
        _animator = animatior;
        _transform = transform;
        _ammoText = ammoText;
        _maxAmmoText = maxAmmoText;
        _hitmarker = hitmarker;
        _inaccuracy = inaccuracy;
        _ai = ai;
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
        player.GetComponent<GunScript>().PickUp(gameObject);
        pickupSound.Play();
    }

    public void Shoot(bool first)
    {
        if (singleReload && reloading)
        {
            cancelReload();
            readyToShoot = false;
            Invoke("ResetShot", timeBetweenShooting);
            return;
        }

        if (!readyToShoot || reloading || bulletsLeft <= 0) return;

        _animator.SetTrigger("shoot");
        readyToShoot = false;

        //rocket launcher
        if (specialWeaponType == 3)
        {
            ShootRocket();
        }

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
        if (hit.distance != 0 && specialWeaponType != 3)
        {
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
                    if (damageFalloffStart < hit.distance)
                    {
                        float distance = hit.distance / 10f - damageFalloffStart / 10f;
                        float distanceMultiplier = (1 - distance * distanceModifier);
                        if (distanceMultiplier > 1) distanceMultiplier = 1;
                        if (distanceMultiplier < 0) distanceMultiplier = 0;
                        finalDamage *= distanceMultiplier;
                    }

                    //final damage (rounded int)
                    var ps = GetComponentInParent<PlayerStats>();
                    var dmg = (collisionObject.GetComponentInParent(typeof(PlayerStats)) as PlayerStats).TakeDamage(
                        (int)finalDamage);
                    ps.damage_done += dmg;

                    if (transform.root.tag == "Player")
                    {
                        GameManager.damageDone += dmg;
                    }

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
                Destroy(bulletHoleClone, 10f);
            }
        }
        else
        {
            //Debug.Log("Not hit");
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
        if (bulletsLeft.Equals(magazineSize) || maxAmmo <= 0 || reloadStatus < 1) return;

        reloading = true;
        readyToShoot = true;

        if (!_ai)
        {
            ZoomOut();
            transform.root.GetComponent<PlayerLook>().Zoom(0);
        }

        if (singleReload)
        {
            reloadStatus = (float)bulletsLeft / (float)magazineSize;
            StartCoroutine(ReloadSingle());
        }
        else
        {
            reloadSound.Play();
            reloadStatus = 0;
            Invoke("ReloadFinished", reloadTime);
        }
    }

    private IEnumerator ReloadSingle()
    {
        while (reloading && maxAmmo > 0 && bulletsLeft < magazineSize)
        {
            reloadSound.Play();
            yield return new WaitForSeconds(singleReloadTime);
            if (!reloading)
            {
                break;
            }

            bulletsLeft += bulletsPerTap;
            maxAmmo -= bulletsPerTap;
            ShowAmmo();
        }

        pickupSound.Play();
        reloading = false;
    }


    private void ReloadFinished()
    {
        if (reloadSound)
        {
            reloadSound.Stop();
        }

        pickupSound.Play();
        currentSpread = initialSpread;
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
        if (reloadStatus < 1)
        {
            return;
        }

        if (SniperHUD)
        {
            SniperHUD.SetActive(true);
            //CrossHair.SetActive(false);
        }
    }

    public void ZoomOut()
    {
        if (SniperHUD)
        {
            SniperHUD.SetActive(false);
            //CrossHair.SetActive(true);
        }
    }

    public float getReloadStatus()
    {
        return reloadStatus;
    }

    public void cancelReload()
    {
        if (reloadSound)
        {
            reloadSound.Stop();
        }

        reloading = false;
        readyToShoot = true;
        reloadStatus = 1;
        CancelInvoke("ReloadFinished");
        StopCoroutine("ReloadSingle");
    }

    public void ShootRocket()
    {
        GetComponent<Launcher>().Launch();
    }
}