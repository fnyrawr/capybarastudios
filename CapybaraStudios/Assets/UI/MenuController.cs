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

    [SerializeField]
    private VisualTreeAsset _settingsButtonTemplate; //
    private VisualElement _settingsButtons;

    //for mute:
    [Header("Mute Button")]
    [SerializeField]
    private Sprite _mutedSprite;
    [SerializeField]
    private Sprite _unmutedSprite;
    private bool _muted;

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
        _settingsButton.clicked += SettingsButtonOnClicked;
        _exitButton.clicked += ExitButtonClicked;
        
        //for mute:
        _muteButton = _doc.rootVisualElement.Q<Button>("MuteButton");
        _muteButton.clicked += MuteButtonOnClicked;
    }

    private void PlayButtonClicked()
    {

        //Debug.Log("a");
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_playButtonTemplate.CloneTree());
        var backButton = _doc.rootVisualElement.Q<Button>("BackButton");
        backButton.clicked += BackButtonOnClicked;
        //Instantiate(_playButtonTemplate);
    }

    private void SettingsButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_settingsButtonTemplate.CloneTree());
        var backButton = _doc.rootVisualElement.Q<Button>("BackButton");
        backButton.clicked += BackButtonOnClicked;
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

    private void MuteButtonOnClicked()
    {
        _muted = !_muted;
        //First grab the image
        var bg = _muteButton.style.backgroundImage;
        //Assign a new Sprite
        bg.value = Background.FromSprite(_muted? _mutedSprite : _unmutedSprite);
        _muteButton.style.backgroundImage = bg;

        //mutes only the menu:
        AudioListener.volume = _muted ? 0 : 1;
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
