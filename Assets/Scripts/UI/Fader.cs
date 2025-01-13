using UnityEngine;
using UnityEngine.UIElements;

public class Fader : MonoBehaviour
{
    #region INTERNAL DATA
    private UIDocument _servicesUI;
    private VisualElement _fader;
    #endregion

    private void Awake()
    {
        _servicesUI = GetComponentInChildren<UIDocument>();

        if (_servicesUI == null)
        {
            DebugUtils.NoComponentError();
        }

        _fader = _servicesUI.rootVisualElement.Q<VisualElement>("Fader");
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.FADE_IN_START, FadeIn);
        EventManager.Subscribe(EventType.FADE_OUT_START, FadeOut);
        _fader.RegisterCallback<TransitionEndEvent>(OnTransitionEndHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.FADE_IN_START, FadeIn);
        EventManager.Unsubscribe(EventType.FADE_OUT_START, FadeOut);
        _fader.UnregisterCallback<TransitionEndEvent>(OnTransitionEndHandler);
    }

    #region EVENT HANDLERS
    private void FadeIn(object data)
    {
        _fader.RemoveFromClassList("fade-out");
        _fader.AddToClassList("fade-in");
    }

    private void FadeOut(object data)
    {
        _fader.RemoveFromClassList("fade-in");
        _fader.AddToClassList("fade-out");
    }

    private void OnTransitionEndHandler(TransitionEndEvent e)
    {
        // If Fade In transition completed
        if (!_fader.ClassListContains("fade-out"))
        {
            EventManager.Trigger(EventType.FADE_IN_END, null);
        }
        // If Fade Out transition completed
        else 
        {
            EventManager.Trigger(EventType.FADE_OUT_END, null);
        }
    }
    #endregion
}
