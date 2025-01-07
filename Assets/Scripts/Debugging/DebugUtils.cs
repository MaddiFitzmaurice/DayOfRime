using UnityEngine;

public static class DebugUtils
{
    public static void HandlerError()
    {
        Debug.LogError("Handler did not receive correct data");
    }

    public static void InspectorFieldError()
    {
        Debug.LogError("Field was not set in inspector");
    }

    public static void NoComponentError()
    {
        Debug.LogError("Component was no attached to GameObject");
    }
}
