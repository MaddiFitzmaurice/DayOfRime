using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TextEffects
{
    private const string TextClass = "text";

    // Create sublines based off of start of text effect notation
    public List<string> CreateSublines(string line)
    {
        return line.Split('[').ToList();
    }

    public (Label, string) ParseEffects(VisualElement container, string subLine)
    {   
        string parsedLine;
        Label textBox;

        // If not end of effect
        if (!subLine.Contains("/"))
        {
            // If Red Effect
            if (subLine.Contains("red]"))
            {
                (textBox, parsedLine) = RedColorTextEffect(subLine);
            }
            // If Small Text Effect
            else if (subLine.Contains("small]"))
            {
                (textBox, parsedLine) = SmallTextEffect(subLine);
            }
            // If no effect
            else 
            {
                (textBox, parsedLine) = (NoTextEffect(), subLine);
            }
        }
        // Else if end of effect
        else 
        {
            (textBox, parsedLine) = RemoveTextEffect(subLine);
        }

        // Add textbox to textbox container
        container.Add(textBox);
        return (textBox, parsedLine);
    }

    private Label CreateTextBox()
    {
        Label textBox = new Label();
        textBox.AddToClassList(TextClass);
        return textBox;
    }

    private Label NoTextEffect()
    {
        return CreateTextBox();
    }

    // Handle red text effect
    private (Label, string) RedColorTextEffect(string subLine)
    {
        string trimmedSubLine = subLine.Replace("red]", "");
        Label textBox = CreateTextBox();
        textBox.AddToClassList("text-color-red");
        return (textBox, trimmedSubLine);
    }

    // Handle small font effect
    private (Label, string) SmallTextEffect(string subLine)
    {
        string trimmedSubLine = subLine.Replace("small]", "");
        Label textBox = CreateTextBox();
        textBox.AddToClassList("text-small");
        return (textBox, trimmedSubLine);
    }

    // Handle text that comes after an effect is finished
    private (Label, string) RemoveTextEffect(string subLine)
    {
        int end = 1;

        foreach (char c in subLine)
        {
            if (c == ']')
            {
                break;
            }
            else 
            {
                end++;
            }
        }

        string trimmedSubLine = subLine.Remove(0, end);
        Label textBox = NoTextEffect();
        return (textBox, trimmedSubLine);
    }
}
