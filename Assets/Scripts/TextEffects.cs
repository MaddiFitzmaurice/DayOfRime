// using System.Collections;
// using UnityEngine;
// using UnityEngine.UIElements;

// public class TextEffects
// {
//    private void AddRedColorEffect(string subLine)
//     {
//         string trimmedSubLine = subLine.Replace("red]", "");
//         Label textBox = new Label();
//         textBox.AddToClassList("text-color-red");
//         AddTextToTextComponents(textBox);
//         StartCoroutine(Typewriter(textBox, trimmedSubLine));
//     }

//     private void AddSmallEffect(string subLine)
//     {
//         string trimmedSubLine = subLine.Replace("small]", "");
//         Label textBox = new Label();
//         textBox.AddToClassList("text-small");
//         AddTextToTextComponents(textBox);
//         StartCoroutine(Typewriter(textBox, trimmedSubLine));
//     }

//     private IEnumerator Typewriter(Label textBox, string subLine)
//     {
//         yield return new WaitForSeconds(0.1f);

//         foreach(char character in subLine)
//         {
//             if (character == '?' || character == '.' || character == ',' || character == ':' ||
//                      character == ';' || character == '!' || character == '-') 
//             {
//                 yield return new WaitForSeconds(0.25f);
//                 //_interpunctuationDelay;
//             }
//             else
//             {
//                 yield return new WaitForSeconds(1f / 60f);
//                 //_simpleDelay;
//             }

//             textBox.text += character;
//         }

//         yield return null;
//     }
// }
