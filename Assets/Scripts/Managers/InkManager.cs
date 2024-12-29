using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkManager : MonoBehaviour
{
    #region INTERNAL DATA
    private Story _currentScript;
    private string _inkLine;
    private bool _hasChoices = false;

    // Ink Script Data
    private const string Action = "a:";
    private const string Character = "c:";
    private const string Portrait = "p:";
    #endregion

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
        _currentScript.RemoveVariableObserver(InkVariableHandler, "state");
    }

    #region EVENT HANDLERS
    private void InkVariableHandler(string variableName, object newValue)
    {
        Debug.Log(_currentScript.variablesState["state"]);
        EventManager.Trigger(EventType.INK_STATE_UPDATE, _currentScript.variablesState["state"]);
    }

    // Assigning the Story with a new TextAsset and
    // displaying the first line
    public void ScriptHandler(object data)
    {
        if (data is TextAsset script)
        {
            _currentScript = new Story(script.text);
            _currentScript.ObserveVariable("state", InkVariableHandler);
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
                _hasChoices = true;
            }
            // If choices have been made
            else if (_hasChoices && _currentScript.currentChoices.Count == 0)
            {
                _hasChoices = false;
                EventManager.Trigger(EventType.INK_CHOICES, _currentScript.currentChoices);
            }
            
            HandleTags(_currentScript.currentTags);
            EventManager.Trigger(EventType.INK_LINES, _inkLine);
        }
        // If no more lines to parse and no questions, signal end of script
        else
        {
           // End of script stuff here
           EventManager.Trigger(EventType.INK_SCRIPT_FIN, null);
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
                // If tag is an action
                if (tag.Contains(Action))
                {
                    if (tag.Contains("enter"))
                    {
                        // _characterState.Character = tag.Remove(0, 8);
                        // _characterState.Toggle = true;
                        //EventManager.EventTrigger(EventType.INK_TOGGLE_CHARACTER, _characterState);
                    }
                    else if (tag.Contains("exit"))
                    {
                        // _characterState.Character = tag.Remove(0, 7);
                        // _characterState.Toggle = false;
                        // EventManager.EventTrigger(EventType.INK_TOGGLE_CHARACTER, _characterState);
                    }
                    else if (tag.Contains("desc"))
                    {
                        // _currentSpeaker.Speaker = "";
                    }
                }
                // If tag is a character
                else if (tag.Contains(Character))
                {
                    // if (tag.Contains("grim") || tag.Contains("gravedigger"))
                    // {
                    //     // _currentSpeaker.Speaker = tag.Remove(0, 2);
                    // }
                    // else if (tag.Contains("nina") || tag.Contains("edward") || tag.Contains("diane")
                    //     || tag.Contains("maureen") || tag.Contains("kenneth"))
                    // {
                    //     // _currentSpeaker.Speaker = "soul";
                    // }
                    // else 
                    // {
                    //     // _currentSpeaker.Speaker = "";
                    // }
                }
                // If tag is a portrait
                else if (tag.Contains(Portrait))
                {
                    // Remove p:
                    // _currentSpeaker.Portrait = tag.Remove(0, 2);
                }
            }
        }
    }
    #endregion
}
