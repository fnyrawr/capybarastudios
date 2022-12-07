using System;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;

    public new Camera camera;
    public Animator animator;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI maxAmmoText;
    public GameObject hitmarker;
    public GameObject bulletHoleGraphic;

    public WeaponAnimationController weaponAnimator;
    public GameObject[] weapons = new GameObject[4];

    public Weapon currentWeapon;
    public int currentSlot = 0;

    private void Awake()
    {
        ChangeWeapon(currentSlot);
    }

    public void EjectGun()
    {
        Debug.Log("drop");
        DitchGun(currentSlot);
        EquipHighest();
    }

    private void EquipHighest()
    {
        foreach (var weapon in weapons)
        {
            if (weapon)
            {
                ChangeWeapon(weapon.GetComponent<Weapon>().weaponSlot - 1);
                return;
            }
        }
        //no weapon on player...
        //TODO
    }

    //gun pickup and discard
    private void DitchGun(int index)
    {
        if (!weapons[index]) return;
        
        if (gunSlot.GetChild(0))
        {
            var oldGun = gunSlot.GetChild(0);
            oldGun.SetParent(null);
            oldGun.GetComponent<Rigidbody>().isKinematic = false;
            oldGun.GetComponent<BoxCollider>().enabled = true;
            print(oldGun.name + " ditched");
        }
        weapons[index] = null;
    }

    public void PickUp(GameObject gun)
    {
        print(gun.name + " aquired");
        var weapon = gun.GetComponent<Weapon>();
        var weaponSlot = weapon.weaponSlot - 1;
        DitchGun(weaponSlot);
        gun.GetComponent<Rigidbody>().isKinematic = true;
        gun.GetComponent<BoxCollider>().enabled = false;
        weapons[weaponSlot] = gun;
        ChangeWeapon(weaponSlot);
    }

    private void HideGun()
    {
        try
        {
            var gun = gunSlot.GetChild(0);
            gun.SetParent(null);
            gun.gameObject.SetActive(false);
        }
        catch (Exception e){}
    }

    private void ChangeWeapon(int index)
    {
        HideGun();
        currentSlot = index;
        weapons[currentSlot].transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        currentWeapon = gunSlot.GetChild(0).GetComponent<Weapon>();
        currentWeapon.init(animator, camera, ammoText, maxAmmoText, hitmarker, bulletHoleGraphic);
        weaponAnimator.refresh();
        weapons[currentSlot].transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapons[currentSlot].transform.localPosition = new Vector3(0, 0, 0);
    }

    public void EquipWeapon(int index)
    {
        if (currentSlot != index)
        {
            ChangeWeapon(index);
        }
    }
}