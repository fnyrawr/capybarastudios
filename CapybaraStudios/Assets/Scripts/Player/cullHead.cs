using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cullHead : MonoBehaviour
{
    public GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        head.layer = 15;
    }

    public void die()
    {
        head.layer = 0;
    }
}