using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class spreadIndicator : MonoBehaviour
{
    private GunScript _gunScript;

    private Vector3 initialSpread;
    [CanBeNull] private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        _gunScript = GetComponentInParent<GunScript>();
        rt = GetComponent<RectTransform>();
        initialSpread = rt.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gunScript.currentWeapon.getReloadStatus() < 1)
        {
            GetComponent<Image>().enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
        }

        rt.localScale = initialSpread * (1 + _gunScript.currentWeapon.currentSpread * 100);
    }
}