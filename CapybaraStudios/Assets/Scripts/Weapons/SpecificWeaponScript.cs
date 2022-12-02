using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecificWeaponScript : MonoBehaviour
{
    public new Camera camera;

    //Gun stats
    public int damage;
    public float spread, range, reloadTime, fireRate, timeBetweenShooting;
    public int maxAmmo, magazineSize, bulletsPerTap;
    public bool hasAmmo, rapidFireEnabled;

    [SerializeField] int bulletsLeft, bulletsShot;
    bool reloading, readyToShoot;

    //hitmarker
    public GameObject hitmarker;

    //bullet hole
    public GameObject bulletHoleGraphic;

    //HUD
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI maxAmmoText;

    private WaitForSeconds rapidFireWait;
    private int controllerMask = ~(1 << 15);
    private Animator _animator;

    private void Awake()
    {
        _animator = transform.parent.parent.GetChild(0).GetComponent<Animator>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
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
            direction = camera.transform.forward + new Vector3(x, y, z);
        }
        else
            direction = camera.transform.forward;

        //hit and damage calc
        if (Physics.Raycast(camera.transform.position, direction, out RaycastHit hit, range,
                controllerMask))
        {
            Debug.Log(hit.transform.name);

            GameObject collisionObject = hit.collider.gameObject;

            if (collisionObject.CompareTag("Head") || collisionObject.CompareTag("Body") ||
                collisionObject.CompareTag("Limbs"))
            {
                //does object have stats?
                if (collisionObject.GetComponent<PlayerStats>() != null)
                {
                    //deal damage
                    float hitMultiplier = 1;
                    if (collisionObject.CompareTag("Head")) hitMultiplier = 3;
                    if (collisionObject.CompareTag("Limbs")) hitMultiplier = 0.75f;
                    int finalDamage = (int)(damage * hitMultiplier);
                    collisionObject.GetComponent<PlayerStats>().TakeDamage(finalDamage);

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
        GameObject bulletHoleClone = Instantiate(bulletHoleGraphic, hit.point, Quaternion.Euler(0, 180, 0));
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
            ammoText.SetText("∞");
            maxAmmoText.SetText("");
        }
        else
        {
            ammoText.SetText(bulletsLeft + " / " + magazineSize);
            if (maxAmmo > 0)
                maxAmmoText.SetText(maxAmmo.ToString());
            else
                maxAmmoText.SetText("0");
        }
    }

    //hitmarker show and disable
    public void HitShow()
    {
        hitmarker.SetActive(true);
    }

    public void HitDisable()
    {
        hitmarker.SetActive(false);
    }
}