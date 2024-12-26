// ENUM FOR ALL EVENTS IN GAME
public enum EventType
{
    #region SCENE EVENTS                     
    PLAY_GAME,
    FADING, 
    QUIT_GAME,  
    MAIN_MENU,
    #endregion

    #region INK EVENTS
    INK_LINES,                  // Sending lines such as dialogue or prose
    INK_CHOICES,               // Sending questions for the player to choose
    #endregion

    #region NARRATIVE EVENTS
    NARRATIVE_SEND_SCRIPT,
    #endregion

    #region GAMEUI EVENTS
    GAMEUI_BUTTON_CLICKED,
    GAMEUI_NEXT_LINE_CLICKED,
    #endregion
};