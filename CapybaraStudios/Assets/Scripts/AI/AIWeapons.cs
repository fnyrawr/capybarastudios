using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    GameObject current;
    Weapon currentWeapon;
    [SerializeField] private WeaponAnimationController weaponAnimator;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform headTransform; //casts ray from this position
    [SerializeField] private Transform gunSlot;
    [SerializeField] private float inaccuracy = 3f;
    private bool isFiring;
    Coroutine fireCoroutine;
    private void Update() {
        if(isFiring && HasWeapon()) {
            currentWeapon.Shoot(true);
        }    
    }

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
        currentWeapon.init(animator, headTransform, null, null, null, inaccuracy);

        weaponAnimator.refresh();
        weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DitchWeapon() {
        if(!HasWeapon()) return;
        var weapon = current.transform;
        weapon.SetParent(null);
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

    public bool HasWeapon() {
        return current != null;
    }

    public void SetFiring(bool val) {
        isFiring = val;
        if(val) {
            fireCoroutine = StartCoroutine(currentWeapon.RapidFire());
        } else {
            if(fireCoroutine != null) StopCoroutine(fireCoroutine);
        }
    }

}
