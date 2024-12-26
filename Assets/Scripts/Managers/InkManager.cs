using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkManager : MonoBehaviour
{
    private Story _currentScript;
    private string _inkLine;

    private void Awake()
    {
        // Event Init
        EventManager.Initialise(EventType.INK_LINES);
        EventManager.Initialise(EventType.INK_CHOICES);
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.NARRATIVE_SEND_SCRIPT, ScriptHandler);
        EventManager.Subscribe(EventType.GAMEUI_BUTTON_CLICKED, ChoiceSelectedHandler);
        EventManager.Subscribe(EventType.GAMEUI_NEXT_LINE_CLICKED, NextLineHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.NARRATIVE_SEND_SCRIPT, ScriptHandler);
        EventManager.Unsubscribe(EventType.GAMEUI_BUTTON_CLICKED, ChoiceSelectedHandler);
        EventManager.Unsubscribe(EventType.GAMEUI_NEXT_LINE_CLICKED, NextLineHandler);
    }

    // Assigning the Story with a new TextAsset and
    // displaying the first line
    public void ScriptHandler(object data)
    {
        if (data is TextAsset script)
        {
            _currentScript = new Story(script.text);
            NextLineHandler(null);
        }
        else
        {
            Debug.LogError("ScriptHandler did not receive TextAsset");
        }
    }

    // Triggering the next line to parse in a Story
    public void NextLineHandler(object data)
    {
        // If there are lines to parse and no questions
        if (_currentScript.canContinue)
        {
            _inkLine = _currentScript.Continue();

            // If no choices to display, handle tags and send next text line
            if (_currentScript.currentChoices.Count != 0)
            {
                EventManager.Trigger(EventType.INK_CHOICES, _currentScript.currentChoices);
            }
            
            HandleTags(_currentScript.currentTags);
            EventManager.Trigger(EventType.INK_LINES, _inkLine);
        }
        // If no more lines to parse and no questions, signal end of script
        else
        {
           
        }
    }

    // Receive an int from the choice button that has been pressed
    public void ChoiceSelectedHandler(object data)
    {
        if (data is int selectedIndex)
        {
            // Tell Story choice that was made, then signal 
            // for proceeding with the story
            _currentScript.ChooseChoiceIndex((int)data);
            NextLineHandler(null);
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }

    // Handling any tags parsed and dealing with them
    public void HandleTags(List<string> currentTags)
    {
        if (currentTags.Count != 0)
        {
            foreach (string tag in currentTags)
            {
                
            }
        }
    }
}
