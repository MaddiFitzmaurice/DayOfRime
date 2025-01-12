using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUI : MonoBehaviour
{
    private VisualElement _ui;  //Root Element
    private Button _playButton;
    private Button _quitButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Get UI Components
        _ui = GetComponent<UIDocument>().rootVisualElement;
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

    #region EVENT HANDLERS
    private void PlayButtonHandler(ClickEvent e)
    {
        EventManager.Trigger(EventType.MAINMENU_OUTRO_START, null);
    }

    private void QuitButtonHandler(ClickEvent e)
    {
        EventManager.Trigger(EventType.QUIT_GAME, null);
    }
    #endregion
}
