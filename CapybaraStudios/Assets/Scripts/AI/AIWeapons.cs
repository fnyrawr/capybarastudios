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
    private float inaccuracy = 3f;
    private bool isFiring;
    Coroutine fireCoroutine;
    [SerializeField] AIAgent agent;

    private void Awake() {
        inaccuracy = agent.config.inaccuracy;
        if(agent.config.startWeapon != null) {
            GameObject weapon = Instantiate(agent.config.startWeapon);
            EquipWeapon(weapon);
        }
    }
    private void Update() {
        if(isFiring && HasWeapon()) {
            currentWeapon.Shoot(true);
        }    
    }

    private void OnTriggerEnter(Collider other) {
        if(!HasWeapon() && other.gameObject.tag == "Weapon"&& !agent.playerStats.dead) {
            EquipWeapon(other.gameObject);
        }
    }

    public void EquipWeapon(GameObject weapon) {
        if(weapon.GetComponent<Weapon>().maxAmmo == 0) return;
        current = weapon;
        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<BoxCollider>().enabled = false;
        weapon.transform.SetParent(gunSlot);
        gunSlot.GetChild(0).gameObject.SetActive(true);
        currentWeapon = gunSlot.GetChild(0).GetComponent<Weapon>();
        currentWeapon.init(animator, headTransform, null, null, null, inaccuracy, true);

        weaponAnimator.refresh();
        weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapon.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DitchWeapon() {
        if (gunSlot.GetChild(0))
        {
            var oldGun = gunSlot.GetChild(0);
            oldGun.GetComponent<Weapon>().cancelReload();
            oldGun.SetParent(null);
            oldGun.GetComponent<Rigidbody>().isKinematic = false;
            oldGun.GetComponent<BoxCollider>().enabled = true;
            print(oldGun.name + " ditched");
        }
        currentWeapon = null;
    }

    public bool HasWeapon() {
        return currentWeapon != null;
    }

    public void SetFiring(bool val) {
        isFiring = val;
        if(val) {
            fireCoroutine = StartCoroutine(currentWeapon.RapidFire());
        } else {
            if(fireCoroutine != null) StopCoroutine(fireCoroutine);
        }
    }

    public void Reload() {
        if(!HasWeapon()) return;
        if(currentWeapon.bulletsLeft > 0) return;
        if(currentWeapon.maxAmmo == 0) {
            DitchWeapon();
            agent.stateMachine.ChangeState(AIStateId.FindWeapon);
            return;
        }
        currentWeapon.Reload();
    }
}