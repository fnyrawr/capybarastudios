using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class spreadIndicator : MonoBehaviour
{
    [SerializeField] private GunScript _gunScript;
    
    private Vector3 initialSpread;
    [CanBeNull] private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        initialSpread = rt.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        rt.localScale = initialSpread * (1 + _gunScript.currentWeapon.currentSpread*100);
    }
}