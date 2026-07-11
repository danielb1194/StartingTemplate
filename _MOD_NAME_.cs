using System;
using MelonLoader;
using Menace.ModpackLoader;
using Menace.SDK;
using static _MOD_NAME_ModSettings;

namespace _MOD_NAME_;

public class _MOD_NAME_ : IModpackPlugin
{
    private static _MOD_NAME_ _instance;
    private MelonLogger.Instance _log;
    private HarmonyLib.Harmony _harmony;

    private const string _logPrefix = "";

    /// <summary>
    /// Logs a message to the mod's logger (MelonLoader's logging system)
    /// </summary>
    /// <param name="message">The message to log</param>
    public static void Log(string message)
    {
        _instance?._log.Msg($"{_logPrefix} {message}");
    }

    /// <summary>
    /// Logs a message to the mod's logger (MelonLoader's logging system) only if the
    /// debug flag IS_DEBUG_LOGGING is set to true.
    /// </summary>
    /// <param name="message">The message to log if debug logging is enabled.</param>
    public static void DebugLog(string message)
    {
        if (GetDebugLogging())
        {
            Log(message);
        }
    }

    /// <summary>
    /// Initializes the mod, setting up the logger, Harmony instance,
    /// and applying necessary patches.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="harmony"></param>
    public void OnInitialize(MelonLogger.Instance logger, HarmonyLib.Harmony harmony)
    {
        _instance = this;
        _log = logger;
        _harmony = harmony;

        // configure the settings
        ConfigureModSettings();

        DebugLog("Applying patch...");

        var patchesApplied = new PatchSet(_harmony, "_MOD_NAME_").Apply();

        DebugLog($"Patches applied: {patchesApplied}");
    }

    /// <summary>
    /// Called when a scene is loaded. This allows the mod to perform
    /// scene-specific logic when the scene changes.
    /// </summary>
    /// <param name="buildIndex">The current scene's build index.</param>
    /// <param name="sceneName">The name of the current scene.</param>
    public void OnSceneLoaded(int buildIndex, string sceneName)
    {
        // This method runs whenever a scene changes. If you have expensive operations that
        // you only want to do, for example, when in the tactical scene, you can
        // use this to check for the specific scene and perform scene-specific logic.
    }
}
