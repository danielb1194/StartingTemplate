using System;
using Il2CppMenace.Tactical;
using Il2CppMenace.Tactical.Skills;
using MelonLoader;
using Menace.ModpackLoader;
using Menace.SDK;
using UnityEngine;
using static _MOD_NAME_ModSettings;

namespace _MOD_NAME_;

public class _MOD_NAME_ : IModpackPlugin
{
    private static _MOD_NAME_ _instance;
    private MelonLogger.Instance _log;
    private HarmonyLib.Harmony _harmony;
    public const string MOD_SETTINGS_GROUP = "_MOD_NAME_";
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
        if (ModSettings.Get<bool>(MOD_SETTINGS_GROUP, DEBUG_LOGGING_KEY))
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
        _MOD_NAME_ModSettings.ConfigureModSettings(MOD_SETTINGS_GROUP);

        DebugLog("Applying patch...");
        var patchesApplied = new PatchSet(_harmony, "_MOD_NAME_").Apply();

        DebugLog($"Patches applied: {patchesApplied}");
    }

    public void OnSceneLoaded(int buildIndex, string sceneName)
    {
        throw new NotImplementedException();
    }
}
