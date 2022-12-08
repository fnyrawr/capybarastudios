using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : Interactable
{
    private Camera _camera;

    //Gun stats
    public int damage;
    public float spread, range, reloadTime, fireRate, timeBetweenShooting, distanceModifier;
    public int maxAmmo, magazineSize, bulletsPerTap;
    public bool hasAmmo, rapidFireEnabled;

    [SerializeField] int bulletsLeft, bulletsShot;
    bool reloading, readyToShoot;

    //hitmarker
    private GameObject _hitmarker;

    //bullet hole
    private GameObject _bulletHoleGraphic;

    //HUD
    private TextMeshProUGUI _ammoText;
    private TextMeshProUGUI _maxAmmoText;

    private WaitForSeconds rapidFireWait;
    private int controllerMask = ~(1 << 15);
    private Animator _animator;
    public int weaponSlot;
    public int animationType;
    public int specialWeaponType;
    // 0 ist für nicht special Weapon
    // 1 ist für Grappling Gun
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        message = "Pick up [E]";
    }

    public void init(Animator animatior, Camera camera, TextMeshProUGUI ammoText, TextMeshProUGUI maxAmmoText,
        GameObject hitmarker, GameObject bulletHoleGraphic)
    {
        _animator = animatior;
        _camera = camera;
        _ammoText = ammoText;
        _maxAmmoText = maxAmmoText;
        _hitmarker = hitmarker;
        _bulletHoleGraphic = bulletHoleGraphic;
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log("Picked up " + gameObject.name);
        player.GetComponent<GunScript>().PickUp(gameObject);
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
            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);
            //Calculate Direction with Spread
            direction = _camera.transform.forward + new Vector3(x, y, z);
        }
        else
            direction = _camera.transform.forward;

        //hit and damage calc
        if (Physics.Raycast(_camera.transform.position, direction, out RaycastHit hit, range,
                controllerMask))
        {
            Debug.Log(hit.transform.name);

            GameObject collisionObject = hit.collider.gameObject;

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
                    float distance = hit.distance / 10f;
                    finalDamage *= (1 - distance * distanceModifier);
                    Debug.Log("distanceMod = " + (1 - distance * distanceModifier));

                    //final damage (rounded int)

                    (collisionObject.GetComponentInParent(typeof(PlayerStats)) as PlayerStats).TakeDamage(
                        (int)finalDamage);

                    //Hitmarker
                    HitShow();
                    Invoke(nameof(HitDisable), 0.2f);
                }
            }
        }
        else
        {
            Debug.Log("Not hit");
        }

        //bullet hole
        GameObject bulletHoleClone = Instantiate(_bulletHoleGraphic, hit.point, Quaternion.Euler(0, 180, 0));
        Destroy(bulletHoleClone, 10f);

        //magazine
        bulletsLeft--;
        bulletsShot--;
        ShowAmmo();
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
        if (rapidFireEnabled)
        {
            while (true)
            {
                Shoot(false);
                yield return rapidFireWait;
            }
        }
        else
        {
            Shoot(true);
            yield return null;
        }
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

        Debug.Log("Reload");
        reloading = true;
        readyToShoot = true;
        Invoke("ReloadFinished", reloadTime);
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
        if (!hasAmmo)
        {
            _ammoText.SetText("∞");
            _maxAmmoText.SetText("");
        }
        else
        {
            _ammoText.SetText(bulletsLeft + " / " + magazineSize);
            if (maxAmmo > 0)
                _maxAmmoText.SetText(maxAmmo.ToString());
            else
                _maxAmmoText.SetText("0");
        }
    }

    //hitmarker show and disable
    public void HitShow()
    {
        _hitmarker.SetActive(true);
    }

    public void HitDisable()
    {
        _hitmarker.SetActive(false);
    }
}