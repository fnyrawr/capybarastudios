using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public Transform firePoint;
    public GameObject rocket;

    public float range = 100f;

    public void Launch()
    {
        GameObject rocketInstance = Instantiate(rocket, firePoint.position, firePoint.rotation);
        rocketInstance.GetComponent<Rigidbody>().AddForce(firePoint.forward * range, ForceMode.Impulse);
    }
}
