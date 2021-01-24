namespace org.openttd
{
    public enum PauseMode
    {
        /// <summary>
        /// A game normally paused
        /// </summary>
        PM_PAUSED_NORMAL         = 1,
        /// <summary>
        /// A game paused for saving/loading
        /// </summary>
        PM_PAUSED_SAVELOAD = 2,
        /// <summary>
        /// A game paused for 'pause_on_join'
        /// </summary>
        PM_PAUSED_JOIN = 4,
        /// <summary>
        /// A game paused because a (critical) error
        /// </summary>
        PM_PAUSED_ERROR = 8,
        /// <summary>
        /// A game paused for 'min_active_clients'
        /// </summary>
        PM_PAUSED_ACTIVE_CLIENTS = 16,
        /// <summary>
        /// A game paused by a game script
        /// </summary>
        PM_PAUSED_GAME_SCRIPT = 32,
    }
}
