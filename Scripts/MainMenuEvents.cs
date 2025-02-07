using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private Button _calculationButton;
    private Button _theoryBytesButton;
    private Button _playNowButton;
    private Button _exitButton;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;
    internal static readonly object MainMenuVisualTree;

    public static Func<IEnumerable<UnityEngine.Object>> postUxmlReload { get; internal set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        // Get the UIDocument component from the GameObject
        _document = GetComponent<UIDocument>();

        // Retrieve the buttons from the UI document
        _calculationButton = _document.rootVisualElement.Q("CalculationButton") as Button;
        _theoryBytesButton = _document.rootVisualElement.Q("TheoryBytesButton") as Button;
        _playNowButton = _document.rootVisualElement.Q("PlayNowButton") as Button;
        _exitButton = _document.rootVisualElement.Q("ExitButton") as Button;

        // Register the click event callbacks for the buttons
        _calculationButton?.RegisterCallback<ClickEvent>(OnCalculationClick);
        _theoryBytesButton?.RegisterCallback<ClickEvent>(OnTheoryBytesClick);
        _playNowButton?.RegisterCallback<ClickEvent>(OnPlayNowClick);
        _exitButton?.RegisterCallback<ClickEvent>(OnExitClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
           }

    private void OnDisable()
    {
        // Unregister the click event callbacks for the buttons
        _calculationButton?.UnregisterCallback<ClickEvent>(OnCalculationClick);
        _theoryBytesButton?.UnregisterCallback<ClickEvent>(OnTheoryBytesClick);
        _playNowButton?.UnregisterCallback<ClickEvent>(OnPlayNowClick);
        _exitButton?.UnregisterCallback<ClickEvent>(OnExitClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }

    }

    private void OnCalculationClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Calculation Button");
        // Add your logic for the Calculation button here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


    private void OnTheoryBytesClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Theory Bytes Button");
        // Add your logic for the Theory Bytes button here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    
    private void OnPlayNowClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Play Now Button");
        // Add your logic for the Play Now button here
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnExitClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Exit Button");
        // Add your logic for exiting the game here
        Application.Quit();
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }
}
