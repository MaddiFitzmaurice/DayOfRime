using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameUI : MonoBehaviour
{
    #region INTERNAL DATA
    // VISUAL ELEMENTS
    private VisualElement _ui;  //Root Element
    private Label _dialogueText;
    private Button _goodButton;
    private Button _badButton;
    #endregion

    private void Awake()
    {
        // Init Events
        EventManager.Initialise(EventType.GAMEUI_BUTTON_CLICKED);

        // Get Components
        _ui = GetComponent<UIDocument>().rootVisualElement;
        _dialogueText = _ui.Q<Label>("Dialogue");
        _goodButton = _ui.Q<Button>("GoodButton");
        _badButton = _ui.Q<Button>("BadButton");
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.INK_LINES, LinesContainerHandler);
        _goodButton.clicked += OnGoodButtonClicked;
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.INK_LINES, LinesContainerHandler);
        _goodButton.clicked -= OnGoodButtonClicked;
    }

    private void OnGoodButtonClicked()
    {
        Debug.Log("HELLO");
    }

    private void OnBadButtonClicked()
    {

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
