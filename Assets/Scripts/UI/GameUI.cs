using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument), typeof(TextAnimations))]
public class GameUI : MonoBehaviour
{
    #region INTERNAL DATA
    // VISUAL ELEMENTS
    private VisualElement _ui;  //Root Element
    private VisualElement _textBoxContainer; // Container that contains all text boxes
    private List<Button> _choiceButtons;
    private Image _nextIcon;
    private Image _skipIcon;
    private const string DisplayClass = "display";
    private bool _choicesShown = false;
    private TextEffects _textEffects;
    private TextAnimations _textAnims;
    #endregion

    private void Awake()
    {
        // Init Events
        EventManager.Initialise(EventType.GAMEUI_BUTTON_CLICKED);

        // Get UI Components
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _textBoxContainer = _ui.Q<VisualElement>("TextComponents");
        _nextIcon = _ui.Q<Image>("NextIcon");
        _skipIcon = _ui.Q<Image>("SkipIcon");
        
        _choiceButtons = new List<Button>
        {
            _ui.Q<Button>("GoodButton"),
            _ui.Q<Button>("BadButton")
        };

        // Get Text Components
        _textEffects = new TextEffects();
        _textAnims = GetComponent<TextAnimations>();
    }

    private void OnEnable()
    {
        // Subscribe to Events
        EventManager.Subscribe(EventType.INK_LINES, InkLineHandler);
        EventManager.Subscribe(EventType.INK_CHOICES, InkChoicesHandler);
        EventManager.Subscribe(EventType.INK_SCRIPT_FIN, ScriptFinHandler);
        
        _nextIcon.RegisterCallback<ClickEvent>(OnNextIconClicked);
        _skipIcon.RegisterCallback<ClickEvent>(OnSkipIconClicked);

        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].RegisterCallback<ClickEvent, int>(OnButtonClicked, i);
        }

        _textAnims.OnAnimComplete += OnAnimCompleteHandler;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.INK_LINES, InkLineHandler);
        EventManager.Unsubscribe(EventType.INK_CHOICES, InkChoicesHandler);
        EventManager.Unsubscribe(EventType.INK_SCRIPT_FIN, ScriptFinHandler);

        _nextIcon.UnregisterCallback<ClickEvent>(OnNextIconClicked);
        _skipIcon.UnregisterCallback<ClickEvent>(OnSkipIconClicked);

        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].UnregisterCallback<ClickEvent, int>(OnButtonClicked);
        }

        _textAnims.OnAnimComplete -= OnAnimCompleteHandler;
    }

    private void Start()
    {
        ShowChoiceButtons(false);
    }

    #region SHOW/HIDE UI
    private void ShowChoiceButtons(bool show)
    {
        foreach (Button button in _choiceButtons)
        {
            if (show)
            {
                button.RemoveFromClassList(DisplayClass);
            }
            else
            {
                button.AddToClassList(DisplayClass);
            }
        }
    }

    private void ShowNextIcon(bool show)
    {
        if (show)
        {
            _nextIcon.RemoveFromClassList(DisplayClass);
        }
        else
        {
            _nextIcon.AddToClassList(DisplayClass);
        }
    }

    private void ShowSkipIcon(bool show)
    {
        if (show)
        {
            _skipIcon.RemoveFromClassList(DisplayClass);
        }
        else
        {
            _skipIcon.AddToClassList(DisplayClass);
        }
    }
    #endregion

    #region TEXT DISPLAYING
    // Display line received by InkManager
    private void DisplayLine(string line)
    {   
        // Clear text boxes from previous line
        _textBoxContainer.Clear();

        // Hold a list of textBoxes and effect-parsed subLines
        List<(Label, string)> textEffects = _textEffects.ParseEffects(_textBoxContainer, line);
        ShowNextIcon(false);
        ShowSkipIcon(true);
        _textAnims.PlayTextAnim(textEffects);
    }

    private void DisplayChoices(List<Choice> choices)
    {
        // Check if correct number of choice
        // NOTE: Can change this to be dynamic later
        if (choices.Count == _choiceButtons.Count)
        {
            int i = 0;
            foreach (Choice choice in choices)
            {
                _choiceButtons[i].text = choice.text;
                i++;
            }
            _choicesShown = true;
        }
        else if (choices.Count == 0)
        {
            _choicesShown = false;
            ShowChoiceButtons(false);
        }
        else 
        {
            Debug.LogError("Button and choice counts do not equal");
        }
    }

    #endregion

    #region EVENT HANDLERS
    // Send index of button that was clicked to correspond to choice index for Ink
    private void OnButtonClicked(ClickEvent e, int buttonIndex)
    {
        EventManager.Trigger(EventType.GAMEUI_BUTTON_CLICKED, buttonIndex);
    }

    private void OnAnimCompleteHandler()
    {
        if (_choicesShown)
        {
            ShowSkipIcon(false);
            ShowNextIcon(false);
            ShowChoiceButtons(true);
        }
        else 
        {
            ShowSkipIcon(false);
            ShowNextIcon(true);
        } 
    }

    private void OnNextIconClicked(ClickEvent e)
    {
        ShowNextIcon(false);
        EventManager.Trigger(EventType.GAMEUI_NEXT_LINE_CLICKED, null);
    }

    private void OnSkipIconClicked(ClickEvent e)
    {
        _textAnims.CancelTextAnim();
    }

    // Display incoming lines
    private void InkLineHandler(object data)
    {
        if (data is string line)
        {
            DisplayLine(line);
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }

    // Display incoming choices
    private void InkChoicesHandler(object data)
    {
        if (data is List<Choice> choices)
        {
            DisplayChoices(choices);
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }

    // What to do when Ink script has finished
    private void ScriptFinHandler(object data)
    {
        ShowChoiceButtons(false);
        ShowNextIcon(false);
        _textBoxContainer.Clear();
        Label text = new Label("We're done here.");
        _textBoxContainer.Add(text);
    }
    #endregion
}
