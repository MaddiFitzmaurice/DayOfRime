using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField] private TextAsset _script;
    private void Awake()
    {
        // Event Init
        EventManager.Initialise(EventType.NARRATIVE_SEND_SCRIPT);

        // Exposed Data Check
        if (_script == null)
        {
            Debug.LogError("Field not set");
        }
    }

    private void Start()
    { 
        EventManager.Trigger(EventType.NARRATIVE_SEND_SCRIPT, _script); 
    }
}
