using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField] private TextAsset _script;
    private void Awake()
    {
        // Event Init
        EventManager.Initialise(EventType.NARRATIVE_SEND_SCRIPT);
    }

    private void Start()
    {
        if (_script != null)
        {
            EventManager.Trigger(EventType.NARRATIVE_SEND_SCRIPT, _script);
        }
    }
}
