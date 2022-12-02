using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;

    private bool hasGun = false;

    public new Camera camera;

    public WeaponAnimationController weaponAnimator;
    public GameObject[] Weapons = new GameObject[4];

    public SpecificWeaponScript currentWeapon;
    public int currentSlot = 0;

    private void Awake()
    {
    }

    void Update()
    {
    }

    public void ejectGun()
    {
        Debug.Log("drop");
        ditchGun(currentSlot);
        equipHighest();
    }

    void equipHighest()
    {
        foreach (var weapon in Weapons)
        {
            if (weapon)
            {
                changeWeapon(weapon.GetComponent<Weapon>().weaponSlot - 1);
                return;
            }
        }
        //no weapon on player...
        //TODO
    }

    //gun pickup and discard
    public void ditchGun(int index)
    {
        if (Weapons[index])
        {
            if (gunSlot.GetChild(0))
            {
                var oldGun = gunSlot.GetChild(0);
                oldGun.SetParent(null);
                oldGun.GetComponent<Rigidbody>().isKinematic = false;
                oldGun.GetComponent<BoxCollider>().enabled = true;
                Debug.Log(oldGun.name + " ditched");
            }

            Weapons[index] = null;
        }
    }

    public void pickUp(GameObject gun)
    {
        Debug.Log(gun.name + " aquired");
        var weaponSlot = gun.GetComponent<Weapon>().weaponSlot - 1;
        ditchGun(weaponSlot);
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<BoxCollider>().enabled = false;
        Weapons[weaponSlot] = gun;
        changeWeapon(weaponSlot);
    }

    public void hideGun()
    {
        try
        {
            var gun = gunSlot.GetChild(0);
            gun.SetParent(null);
            gun.gameObject.SetActive(false);
        }
        catch (UnityException e)
        {
        }
    }

    public void changeWeapon(int index)
    {
        hideGun();
        currentSlot = index;
        Weapons[currentSlot].transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        currentWeapon = gunSlot.GetChild(0).GetComponent<SpecificWeaponScript>();
        weaponAnimator.refresh();
        Weapons[currentSlot].transform.localRotation = Quaternion.Euler(0, 0, 0);
        Weapons[currentSlot].transform.localPosition = new Vector3(0, 0, 0);
    }

    public void EquipWeapon(int index)
    {
        if (currentSlot != index)
        {
            changeWeapon(index);
        }
    }
}