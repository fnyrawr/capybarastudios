using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    GameObject current;
    Weapon currentWeapon;
    [SerializeField] private WeaponAnimationController weaponAnimator;
    [SerializeField] private Transform gunSlot;

    private void OnTriggerEnter(Collider other) {
        if(!HasWeapon() && other.gameObject.tag == "Weapon") {
            EquipWeapon(other.gameObject);
        }
    }

    public void EquipWeapon(GameObject weapon) {
        current = weapon;
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().enabled = false;
        weapon.transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        currentWeapon = gunSlot.GetChild(0).GetComponent<Weapon>();
        //currentWeapon.init(animator, camera, ammoText, maxAmmoText, hitmarker, bulletHoleGraphic);

        weaponAnimator.refresh();
        weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DitchWeapon() {
        if(!HasWeapon()) return;
        print("aaa");
        Debug.Log(current.transform);
        var weapon = current.transform;
        weapon.SetParent(null);
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<BoxCollider>().enabled = true;
        //current = null;
        //currentWeapon = null;
    }

    public bool HasWeapon() {
        return current != null;
    }

}
