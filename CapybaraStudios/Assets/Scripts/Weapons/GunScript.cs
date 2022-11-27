using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;
    private bool hasPrimary = false;
    private bool hasSecondary = false;

    private bool hasGun = false;

    public new Camera camera;

    public WeaponAnimationController weaponAnimator;
    public GameObject[] Weapons = new GameObject[4];
    public GameObject[] PrimaryWeapons = new GameObject[4];
    public int selectedWeapon = 0;
    public int selectedPrimaryWeapon = 0;
    
    private void Awake()
    {
    }

    void Update()
    {
    }

    //gun pickup and discard
    public void ditchGun(int weaponType)
    {
        if (Weapons[weaponType])
        {
            if (gunSlot.GetChild(0))
            {
                var oldGun = gunSlot.GetChild(0);
                oldGun.SetParent(null);
                oldGun.GetComponent<Rigidbody>().isKinematic = false;
                oldGun.GetComponent<BoxCollider>().enabled = true;
                Debug.Log(oldGun.name + " ditched");
            }
        }
    }

    public void pickUp(GameObject gun)
    {
        Debug.Log(gun.name + " aquired");
        var weaponType = gun.GetComponent<Weapon>().weaponType;
        ditchGun(weaponType);
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<BoxCollider>().enabled = false;
        Weapons[weaponType] = gun;
        changeWeapon(weaponType);
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

    public void changeWeapon(int weaponType)
    {
        hideGun();
        selectedWeapon = weaponType;
        Weapons[weaponType].transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        weaponAnimator.refresh();
        Weapons[weaponType].transform.localRotation = Quaternion.Euler(0, 0, 0);
        Weapons[weaponType].transform.localPosition = new Vector3(0, 0, 0);
    }

    public void EquipWeapon(int index)
    {
        foreach (var item in Weapons)
        {
            item.SetActive(false);
        }
        Weapons[index].SetActive(true);
        selectedWeapon = index;
    }

    public void EquipPrimary(int index)
    {
        //equip primary slot
        EquipWeapon(0);
        /*
        assault rifle = 0
        shotgun = 1
        submachine gun = 2
        machine gun = 3
        */
        foreach (var item in PrimaryWeapons)
        {
            item.SetActive(false);
        }
        PrimaryWeapons[index].SetActive(true);
        selectedPrimaryWeapon = index;
    }

}