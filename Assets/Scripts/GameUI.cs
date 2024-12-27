using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUI : MonoBehaviour
{
    #region INTERNAL DATA
    // VISUAL ELEMENTS
    private VisualElement _ui;  //Root Element
    private Label _dialogueText;
    private List<Button> _choiceButtons;
    private Image _nextIcon;
    private const string _displayClass = "display";
    #endregion

    private void Awake()
    {
        // Init Events
        EventManager.Initialise(EventType.GAMEUI_BUTTON_CLICKED);

        // Get Components
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _dialogueText = _ui.Q<Label>("Dialogue");
        _nextIcon = _ui.Q<Image>("NextIcon");

        _choiceButtons = new List<Button>
        {
            _ui.Q<Button>("GoodButton"),
            _ui.Q<Button>("BadButton")
        };
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.INK_LINES, LinesContainerHandler);
        EventManager.Subscribe(EventType.INK_CHOICES, ChoicesContainerHandler);
        EventManager.Subscribe(EventType.INK_SCRIPT_FIN, ScriptFinHandler);
        
        _nextIcon.RegisterCallback<ClickEvent>(OnNextIconClicked);

        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].RegisterCallback<ClickEvent, int>(OnButtonClicked, i);
        }
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.INK_LINES, LinesContainerHandler);
        EventManager.Unsubscribe(EventType.INK_CHOICES, ChoicesContainerHandler);
        EventManager.Unsubscribe(EventType.INK_SCRIPT_FIN, ScriptFinHandler);

        _nextIcon.UnregisterCallback<ClickEvent>(OnNextIconClicked);

        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].UnregisterCallback<ClickEvent, int>(OnButtonClicked);
        }
    }

    private void Start()
    {
        ShowChoiceButtons(false);
    }

    private void ShowChoiceButtons(bool show)
    {
        foreach (Button button in _choiceButtons)
        {
            if (show)
            {
                button.RemoveFromClassList(_displayClass);
            }
            else
            {
                button.AddToClassList(_displayClass);
            }
        }
    }

    private void ShowNextIcon(bool show)
    {
        if (show)
        {
            _nextIcon.RemoveFromClassList(_displayClass);
        }
        else
        {
            _nextIcon.AddToClassList(_displayClass);
        }
    }

    #region EVENT HANDLERS
    // Send index of button that was clicked to correspond to choice index for Ink
    private void OnButtonClicked(ClickEvent e, int buttonIndex)
    {
        EventManager.Trigger(EventType.GAMEUI_BUTTON_CLICKED, buttonIndex);
    }

    private void OnNextIconClicked(ClickEvent e)
    {
        EventManager.Trigger(EventType.GAMEUI_NEXT_LINE_CLICKED, null);
    }

    // Display incoming lines
    private void LinesContainerHandler(object data)
    {
        if (data is string line)
        {
            _dialogueText.text = line;
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }

    // Display incoming choices
    private void ChoicesContainerHandler(object data)
    {
        if (data is List<Choice> choices)
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
                ShowChoiceButtons(true);
                ShowNextIcon(false);
            }
            else if (choices.Count == 0)
            {
                ShowChoiceButtons(false);
                ShowNextIcon(true);
            }
            else 
            {
                Debug.LogError("Button and choice counts do not equal");
            }
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }

    private void ScriptFinHandler(object data)
    {
        ShowChoiceButtons(false);
        ShowNextIcon(false);
        _dialogueText.text = "Demo Finished";
    }
    #endregion
}
