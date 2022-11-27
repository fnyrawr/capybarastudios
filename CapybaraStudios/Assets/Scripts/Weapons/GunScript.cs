using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations.Rigging;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;
    public TwoBoneIKConstraint leftTarget;
    public RigBuilder rigBuilder;
    private bool hasPrimary = false;
    private bool hasSecondary = false;

    private bool hasGun = false;

    public new Camera camera;


    public Animator animator;
    public GameObject[] Weapons = new GameObject[4];
    public int selectedWeapon = 0;

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
            var oldGun = gunSlot.GetChild(0);
            oldGun.SetParent(null);
            oldGun.GetComponent<Rigidbody>().isKinematic = false;
            oldGun.GetComponent<BoxCollider>().enabled = true;
            Debug.Log(oldGun.name + " ditched");
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
        animator.SetInteger("weaponType", weaponType);
        Weapons[weaponType].transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        var temp = Weapons[weaponType].transform.Find("ref_left_hand_target");
        leftTarget.data.target = temp ? temp : null;
        rigBuilder.Build();
        Weapons[weaponType].transform.localRotation = Quaternion.Euler(0, 0, 0);
        Weapons[weaponType].transform.localPosition = new Vector3(0, 0, 0);
    }

    public void EquipKnife()
    {
    }

    public void EquipPrimary()
    {
    }

    public void EquipSecondary()
    {
    }
}