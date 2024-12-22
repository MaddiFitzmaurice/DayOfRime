using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUI : MonoBehaviour
{
    #region INTERNAL DATA
    // VISUAL ELEMENTS
    private VisualElement _ui;  //Root Element
    private Label _dialogueText;
    #endregion

    private void Awake()
    {
        // Get Components
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _dialogueText = _ui.Q<Label>("Dialogue");
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.INK_LINES, LinesContainerHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.INK_LINES, LinesContainerHandler);
    }

    // Display incoming lines
    private void LinesContainerHandler(object data)
    {
        if (data is string line)
        {
            _dialogueText.text = line;
        }
    }

    // Display incoming questions
}
