using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;

    void Start()
    {
        StartCoroutine(LateStart(0.5f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
    }
}