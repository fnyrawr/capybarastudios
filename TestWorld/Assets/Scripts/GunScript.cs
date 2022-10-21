using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera camera;

    public GameObject hitmarker;
    public float distance;


    void Update()
    {
        
    }

    public void Shoot()
    {
        Debug.Log("Shoot!");

        RaycastHit hit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.tag == "Enemy")
            {
                HitShow();
                Invoke("HitDisable", 0.2f);
            }
        }
        else {
            Debug.Log("Not hit");
        }
            




    }

    public void HitShow()
    {
        hitmarker.SetActive(true);
    }
    public void HitDisable()
    {
        hitmarker.SetActive(false);
    }
}
