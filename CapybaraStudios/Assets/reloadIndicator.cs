using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class reloadIndicator : MonoBehaviour
{
    [SerializeField] private GunScript _gunScript;

    [CanBeNull] private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var status = _gunScript.getReloadStatus();
        if (status == 1)
        {
            img.enabled = false;
        }
        else
        {
            img.enabled = true;
            img.fillAmount = status;
        }
    }
}