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
    public int magazineSize, bulletsPerTap;
    public bool rapidFireEnabled;

    int bulletsLeft, bulletsShot;
    bool reloading, readyToShoot;

    //hitmarker
    public GameObject hitmarker;

    //bullet hole
    public GameObject bulletHoleGraphic;

    //HUD
    public TextMeshProUGUI ammoText;


    private WaitForSeconds rapidFireWait;
    private int controllerMask = ~(1 << 15);

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
    }

    void Update()
    {
        //update ammo
        //ammoText.SetText(bulletsLeft + " / " + magazineSize);
    }

    public void Shoot()
    {
        if (reloading) Debug.Log("Reloading");
        if (bulletsLeft <= 0) Debug.Log("no Bullets left");
        if (!readyToShoot) Debug.Log("weapon on cooldown");
        if (!readyToShoot || reloading || bulletsLeft <= 0) return;

        Debug.Log("Shoot!");

        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        float z = Random.Range(-spread, spread);
        //Calculate Direction with Spread
        Vector3 direction = camera.transform.forward + new Vector3(x, y, z);

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
        ammoText.SetText(bulletsLeft + " / " + magazineSize);
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            readyToShoot = true;
            Shoot();
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
                Shoot();
                yield return rapidFireWait;
            }
        }
        else
        {
            Shoot();
            yield return null;
        }
    }

    //reload
    public void Reload()
    {
        Debug.Log("Reload");
        reloading = true;
        readyToShoot = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        ammoText.SetText(bulletsLeft + " / " + magazineSize);
        reloading = false;
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
