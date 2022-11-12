using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshUpdater : MonoBehaviour
{
    private MeshCollider mc;
    private SkinnedMeshRenderer smr;

    private MeshFilter mf;

    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<MeshCollider>();
        smr = GetComponent<SkinnedMeshRenderer>();
        mf = GetComponent<MeshFilter>();
    }


    // Update is called once per frame
    void Update()
    {
        smr.BakeMesh(mf.mesh);
        mc.sharedMesh = mf.mesh;
    }
}