using System;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{
    public Transform gunSlot;

    public Camera camera;
    public Animator animator;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI maxAmmoText;
    public GameObject hitmarker;
    public WeaponAnimationController weaponAnimator;
    public GameObject[] weapons = new GameObject[3];
    public Weapon currentWeapon;
    public int currentSlot = 0;
    Coroutine fireCoroutine;
    private PlayerLook cameraScript;

    private void Awake()
    {
        cameraScript = GetComponent<PlayerLook>();
        ChangeWeapon(currentSlot);
    }

    public void EjectGun()
    {
        if(currentSlot == 2) return;
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
            print("ditched");
            StopSpecial();
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
        if(currentWeapon.specialWeaponType == 1) {
            gun.GetComponent<GrapplingGun>().init(camera, GetComponent<PlayerLook>(),  GetComponent<CharacterController>(), GetComponent<PlayerMovement>());
        }
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
        if(weapons[currentSlot] != null) {
            StopSpecial();
            HideGun();
        }
        currentSlot = index;
        weapons[currentSlot].transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        currentWeapon = gunSlot.GetChild(0).GetComponent<Weapon>();
        currentWeapon.init(animator, camera.transform, ammoText, maxAmmoText, hitmarker);
        weaponAnimator.refresh();
        weapons[currentSlot].transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapons[currentSlot].transform.localPosition = new Vector3(0, 0, 0);
        currentWeapon.ShowAmmo();
    }

    public void StartSpecial() {
        switch(currentWeapon.specialWeaponType)
        {
            case 0: //normal
                print(currentWeapon.zoom);
                cameraScript.Zoom(currentWeapon.zoom);
                break;
            case 1: //grapling gun
                weapons[currentSlot].GetComponent<GrapplingGun>().Hook();
                break;
            case 2: //sniper
                cameraScript.Zoom(currentWeapon.zoom);
                currentWeapon.ZoomIn();
                break;
            default:
                break;
        }
    }

    public void StopSpecial() {
        switch(currentWeapon.specialWeaponType)
        {
            case 0: //normal
                cameraScript.Zoom(0f);
                break;
            case 1: //grapling gun
                weapons[currentSlot].GetComponent<GrapplingGun>().StopHook();
                break;
            case 2: //sniper
                cameraScript.Zoom(0f);
                currentWeapon.ZoomOut();
                break;
            default:
                break;
        }
    }

    public void EquipWeapon(int index)
    {
        if (currentSlot != index)
        {
            ChangeWeapon(index);
        }
    }

    public void StopFiring() {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
    }

    public void StartFiring()
    {
        //fireCoroutine = StartCoroutine(gun.RapidFire());
        fireCoroutine = StartCoroutine(currentWeapon.RapidFire());
    }
    public void Reload() {
        currentWeapon.Reload();
    }

    public void Shoot() {
        currentWeapon.Shoot(true);
    }

}