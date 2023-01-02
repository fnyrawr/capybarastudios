using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    // Start is called before the first frame update


    public TextMeshProUGUI damageText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI timeText;

    // Update is called once per frame
    void Update()
    {
        timeText.text = GameManager.time;
        damageText.text = GameManager.damageDone + "";
        killsText.text = GameManager.kills + "";
    }
}