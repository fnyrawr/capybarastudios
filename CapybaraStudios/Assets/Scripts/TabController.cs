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
<<<<<<< HEAD


        //timeText.text = Time.timeSinceLevelLoad + "";
        timeText.text = string.Format("{0:00}", Time.timeSinceLevelLoad);
        
=======
        timeText.text = FindObjectOfType<GameManager>().time;
        damageText.text = GameManager.damageDone + "";
        killsText.text = GameManager.kills + "";
>>>>>>> f504a30633903a588fe8f9b925528e5b5051748d
    }
}