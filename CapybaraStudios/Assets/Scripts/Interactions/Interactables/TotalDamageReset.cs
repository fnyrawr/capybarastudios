using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalDamageReset : Interactable
{
    public GameObject button;

    public TextMeshPro totalDamageText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected override void Interact(GameObject player)
    {
        Debug.Log(player.name + "interacted with " + gameObject.name);
        var buttonRenderer = button.GetComponent<Renderer>();
        buttonRenderer.material.SetColor("_Color", Color.green);
        Invoke("ChangeColorBack", 0.5f);
        totalDamageText.text = "0";
    }

    private void ChangeColorBack()
    {
        var buttonRenderer = button.GetComponent<Renderer>();
        buttonRenderer.material.SetColor("_Color", Color.red);
    }
}