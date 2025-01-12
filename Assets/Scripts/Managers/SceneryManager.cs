using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    #region EXTERNAL DATA
    [Header("Window States")]
    [SerializeField] private List<SpriteState> _statesList;
    #endregion

    #region INTERNAL DATA
    private Dictionary<NarrativeState, Sprite> _statesDict;
    private SpriteRenderer _backgroundRenderer;
    #endregion

    private void Awake()
    {
        if (_statesList.Count == 0)
        {
            DebugUtils.InspectorFieldError();
        }

        _statesDict = new Dictionary<NarrativeState, Sprite>();
        ConvertToDict();

        // Component
        _backgroundRenderer = GetComponentInChildren<SpriteRenderer>();
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
        foreach (SpriteState spriteState in _statesList)
        {
            _statesDict.Add(spriteState.NarrativeState, spriteState.Sprite);
        }
    }

    private void UpdateScenery(int state)
    {
        _backgroundRenderer.sprite = _statesDict[(NarrativeState)state];
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
