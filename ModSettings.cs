using Menace.SDK;

public static class _MOD_NAME_ModSettings
{
    public static readonly bool DEFAULT_IS_DEBUG_LOGGING = false;

    public static readonly string DEBUG_LOGGING_KEY = "_MOD_NAME_DebugLogging";

    /// <summary>
    /// Configures the mod settings for the _MOD_NAME_ Mod.
    /// </summary>
    /// <param name="modSettingsGroup">
    /// The group name under which the mod settings should be registered.
    /// </param>
    public static void ConfigureModSettings(string modSettingsGroup)
    {
        ModSettings.Register(
            modSettingsGroup,
            settings =>
            {
                // Settings for accuracy when flanking
                settings.AddHeader("_MOD_NAME_ Logging Settings");
                settings.AddToggle(
                    DEBUG_LOGGING_KEY,
                    "Enable or disable debug logging for the _MOD_NAME_ mod.",
                    DEFAULT_IS_DEBUG_LOGGING
                );
            }
        );
    }
}
