using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyMapper : MonoBehaviour
{
    public static PlayerInput playerInput;

    public void Awake()
    {
        LoadRebinds();
    }

    private static void LoadRebinds()
    {
        var tmp = FindObjectOfType<InputManager>();
        if (tmp)
        {
            playerInput = tmp.playerInput;
        }
        else
        {
            playerInput = new PlayerInput();   
            var rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
            playerInput.LoadBindingOverridesFromJson(rebinds);
        }
    }

    public static void SaveRebinds()
    {
        var rebinds = playerInput.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("rebinds", rebinds);
    }

    public static void ResetBindings()
    {
        PlayerPrefs.DeleteKey("rebinds");
        playerInput = new PlayerInput();
        foreach (var keyLabeler in FindObjectsOfType<KeyLabeler>())
        {
            keyLabeler.Label();
        }

        try
        {
            FindObjectsOfType<KeyLabeler>()[0].NotifyInputManager();
        }
        catch (Exception _)
        {
        }
    }
}