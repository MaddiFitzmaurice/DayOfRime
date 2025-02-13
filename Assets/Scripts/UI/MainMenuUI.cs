using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUI : MonoBehaviour
{
    private VisualElement _ui;  //Root Element
    private UIDocument _uiDoc;
    private Button _playButton;
    private Button _quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Get UI Components
        _uiDoc = GetComponent<UIDocument>();
        _ui = _uiDoc.rootVisualElement;
        _playButton = _ui.Q<Button>("PlayButton");
        _quitButton = _ui.Q<Button>("QuitButton");
    }

    private void OnEnable()
    {
        _playButton.RegisterCallback<ClickEvent>(PlayButtonHandler);
        _quitButton.RegisterCallback<ClickEvent>(QuitButtonHandler);
    }

    private void OnDisable()
    {
        _playButton.UnregisterCallback<ClickEvent>(PlayButtonHandler);
        _quitButton.UnregisterCallback<ClickEvent>(QuitButtonHandler);
    }

    private void Start()
    {
        _uiDoc.gameObject.SetActive(true);
    }

    #region EVENT HANDLERS
    private void PlayButtonHandler(ClickEvent e)
    {
        _uiDoc.gameObject.SetActive(false);
        EventManager.Trigger(EventType.MAINMENU_OUTRO_START, null);
    }

    private void QuitButtonHandler(ClickEvent e)
    {
        _uiDoc.gameObject.SetActive(false);
        EventManager.Trigger(EventType.QUIT_GAME, null);
    }
    #endregion
}
