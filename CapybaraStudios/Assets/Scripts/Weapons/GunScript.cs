using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;
    public TwoBoneIKConstraint rightTarget;
    public TwoBoneIKConstraint leftTarget;
    public RigBuilder rigBuilder;

    private bool hasGun = false;

    public new Camera camera;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot, reloading;

    //hitmarker
    public GameObject hitmarker;

    //bullet hole
    public GameObject bulletHoleGraphic;

    //HUD
    public TextMeshProUGUI ammoText;



    private int controllerMask = ~(1 << 15);

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    void Update()
    {
        //update ammo
        ammoText.SetText(bulletsLeft + " / " + magazineSize);
    }

    public void Shoot()
    {
        if (!readyToShoot) Debug.Log("not ready to shoot");
        if (!shooting) Debug.Log("not shooting");
        if (reloading) Debug.Log("Reloading");
        if (bulletsLeft <= 0) Debug.Log("no Bullets left");
        if (!readyToShoot || reloading || bulletsLeft <= 0) return;

        Debug.Log("Shoot!");

        bulletsShot = bulletsPerTap;

        //Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);
        //Calculate Direction with Spread
        Vector3 direction = camera.transform.forward + new Vector3(x, y, 0);

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
                if (collisionObject.GetComponent<PlayerStats>() == null) return;
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
        else
        {
            Debug.Log("Not hit");
        }

        //bullet hole
        Instantiate(bulletHoleGraphic, hit.point, Quaternion.Euler(0, 180, 0));

        //magazine
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }

    }


//fire speed
    private void ResetShot()
    {
        readyToShoot = true;
    }

    //reload
    public void Reload()
    {
        Debug.Log("Reload");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
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

    //gun pickup and discard
    public void ditchGun()
    {
        if (hasGun)
        {
            var oldGun = gunSlot.GetChild(0);
            oldGun.SetParent(null);
            oldGun.GetComponent<Rigidbody>().isKinematic = false;
            oldGun.GetComponent<BoxCollider>().enabled = true;
            Debug.Log(oldGun.name + " ditched");
        }
    }

    public void pickUp(GameObject gun)
    {
        ditchGun();
        Debug.Log(gun.name + " aquired");
        hasGun = true;
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<BoxCollider>().enabled = false;
        gun.transform.SetParent(gunSlot);
        leftTarget.data.target = gun.transform.Find("ref_left_hand_target");
        rigBuilder.Build();
        gun.transform.localRotation = Quaternion.Euler(0, 0, 0);
        gun.transform.localPosition = new Vector3(0, 0, 0);
    }
}