using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRespawn : MonoBehaviour
{
    public void triggerRespawn()
    {
        StartCoroutine(WaitAndRespawn());
    }


    private IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSeconds(3);
        GameManager.triggerRespawn(transform.position);
        Destroy(gameObject);
    }
}