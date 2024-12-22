// ENUM FOR ALL EVENTS IN GAME
public enum EventType
{
    #region SCENE MANAGEMENT                      
    PLAY_GAME,
    FADING, 
    QUIT_GAME,  
    MAIN_MENU,
    #endregion

    #region INK MANAGEMENT
    INK_LINES,                  // Sending lines such as dialogue or prose
    INK_QUESTIONS,               // Sending questions for the player to choose
    #endregion

    #region NARRATIVE MANAGEMENT
    NARRATIVE_SEND_SCRIPT,
    #endregion
};