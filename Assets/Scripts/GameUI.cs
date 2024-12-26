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

        _nextIcon.UnregisterCallback<ClickEvent>(OnNextIconClicked);

        for (int i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].UnregisterCallback<ClickEvent, int>(OnButtonClicked);
        }
    }

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
}
