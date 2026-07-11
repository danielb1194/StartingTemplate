using Menace.SDK;

public static class _MOD_NAME_ModSettings
{
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
                settings.AddHeader("_MOD_NAME_ Settings");
            }
        );
    }
}
