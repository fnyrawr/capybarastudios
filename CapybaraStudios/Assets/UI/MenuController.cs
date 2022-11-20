using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    //Attributes
    private UIDocument _doc; // Reference to UiDocument

    private VisualElement _buttonsWrapper;

    //
    [SerializeField]
    private VisualTreeAsset _playButtonTemplate; //
    private VisualElement _playButtons; //
    private Button _playButton;

    private Button _settingsButton;
    private Button _exitButton;
    private Button _muteButton;
    private Button backButton;

    private void Awake()
    {
        _doc = GetComponent<UIDocument>(); //Grab and store the UIDocument

        //Querx to get the Button
        //First Access the root visual element, 
        //then with Q method query the actual button element
        //Q method needs the Typ of the object we want to get and optionaly name
        _playButton = _doc.rootVisualElement.Q<Button>("PlayButton");
        _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
        _exitButton = _doc.rootVisualElement.Q<Button>("ExitButton");
        _muteButton = _doc.rootVisualElement.Q<Button>("MuteButton");
        
        //Buttons Wrapper
        _buttonsWrapper = _doc.rootVisualElement.Q<VisualElement>("Buttons");

        _playButton.clicked += PlayButtonClicked; // () => { DoSomething(); };
        _exitButton.clicked += ExitButtonClicked;
        
        //backButton.clicked += BackButtonOnClicked;

        //
        //_playButtons = _playButtonTemplate.CloneTree();
        
        
        
    }

    private void PlayButtonClicked()
    {

        Debug.Log("a");
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButtonTemplate.CloneTree());
        var backButton = _doc.rootVisualElement.Q<Button>("BackButton");
        backButton.clicked += BackButtonOnClicked;
        //Instantiate(_playButtonTemplate);
    }

    private void BackButtonOnClicked()
    {
    _buttonsWrapper.Clear();
    _buttonsWrapper.Add(_playButton);
    _buttonsWrapper.Add(_settingsButton);
    _buttonsWrapper.Add(_exitButton);

    }

    private void ExitButtonClicked()
    {
        Application.Quit();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
