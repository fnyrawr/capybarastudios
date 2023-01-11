using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyLabeler : MonoBehaviour
{
    /**public InputActionAsset playerInput;
    public string action;
    public int index = 0;
    public InputBinding ib;

    public void Start()
    {
        getLabel();
    }

    public void OnEnable()
    {
        getLabel();
    }


    public static InputBinding getBinding(InputActionAsset actionAsset, string action, int index)
    {
        var tmp = 0;
        foreach (var playerInputActionMap in actionAsset.actionMaps)
        {
            foreach (var inputBinding in playerInputActionMap.bindings)
            {
                //print(inputBinding);
                if (inputBinding.ToString().Contains(action))
                {
                    if (tmp index)
                    {
                        tmp++;
                        continue;
                    }

                    return inputBinding;
                }
            }
        }

        return new InputBinding();
    }

    public void getLabel()
    {
        var inputBinding = getBinding(playerInput, action, index);
        GetComponentTextMeshPro>().text = inputBinding.path.Split("/")[1].ToUpper();
        ib = inputBinding;
    }**/
    [SerializeField] private InputActionReference action = null;

    [SerializeField] private int index;
    private TMP_Text bindingDisplayNameText = null;

    //[SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private const string RebindsKey = "rebinds";

    private void Start()
    {
        bindingDisplayNameText = GetComponentInChildren<TMP_Text>();
        Label();
    }

    private static void Save()
    {
        KeyMapper.SaveRebinds();
    }

    public void StartRebinding()
    {
        waitingForInputObject.SetActive(true);

        rebindingOperation = KeyMapper.playerInput.FindAction(action.name).PerformInteractiveRebinding(index)
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
            KeyMapper.playerInput.FindAction(action.name).bindings[index].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        rebindingOperation.Dispose();
        Save();
        waitingForInputObject.SetActive(false);
    }

    public void Label()
    {
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
            KeyMapper.playerInput.FindAction(action.name).bindings[index].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}