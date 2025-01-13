// ENUM FOR ALL EVENTS IN GAME
public enum EventType
{
    #region SERVICES EVENTS                    
    PLAY_GAME,
    QUIT_GAME,  
    MAIN_MENU,
    FADE_OUT_START,
    FADE_OUT_END,
    FADE_IN_START,
    FADE_IN_END,
    #endregion

    #region INK EVENTS
    INK_LINES,                  // Sending lines such as dialogue or prose
    INK_CHOICES,               // Sending questions for the player to choose
    INK_STATE_UPDATE,
    INK_SCRIPT_FIN,
    #endregion

    #region NARRATIVE EVENTS
    NARRATIVE_SEND_SCRIPT,
    #endregion

    #region GAMEUI EVENTS
    GAMEUI_BUTTON_CLICKED,
    GAMEUI_NEXT_LINE_CLICKED,
    GAMEUI_SKIP_LINE_CLICKED,
    #endregion

    #region MAIN MENU
    MAINMENU_OUTRO_START,
    #endregion
};