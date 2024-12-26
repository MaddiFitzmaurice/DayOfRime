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
}
