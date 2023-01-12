using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class M_HeadCuller : NetworkBehaviour
{
    [SerializeField] private GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner) return;
        head.layer = 15;
    }

    public void die()
    {
        head.layer = 0;
    }
}
