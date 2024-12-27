using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    #region EXTERNAL DATA
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer _window;
    [Header("Window States")]
    [SerializeField] private List<SpriteState> _windowStatesList;
    #endregion

    #region INTERNAL DATA
    private Dictionary<NarrativeState, Sprite> _windowStatesDict;
    #endregion

    private void Awake()
    {
        if (_window == null)
        {
            DebugUtils.InspectorFieldError();
        }

        if (_windowStatesList.Count == 0)
        {
            DebugUtils.InspectorFieldError();
        }

        _windowStatesDict = new Dictionary<NarrativeState, Sprite>();
        ConvertToDict();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.INK_STATE_UPDATE, InkVariableHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.INK_STATE_UPDATE, InkVariableHandler);
    }

    private void ConvertToDict()
    {
        foreach (SpriteState spriteState in _windowStatesList)
        {
            _windowStatesDict.Add(spriteState.NarrativeState, spriteState.Sprite);
        }
    }

    private void UpdateScenery(int state)
    {
        _window.sprite = _windowStatesDict[(NarrativeState)state];
    }

    #region EVENT HANDLERS
    private void InkVariableHandler(object data)
    {
        if (data is int state)
        {
            UpdateScenery(state);
        }
        else 
        {
            DebugUtils.HandlerError();
        }
    }
    #endregion
}
