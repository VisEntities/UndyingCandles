/*
 * Copyright (C) 2024 Game4Freak.io
 * This mod is provided under the Game4Freak EULA.
 * Full legal terms can be found at https://game4freak.io/eula/
 */

using HarmonyLib;
using Oxide.Core;

namespace Oxide.Plugins
{
    [Info("Undying Candles", "VisEntities", "1.0.0")]
    [Description(" ")]
    public class UndyingCandles : RustPlugin
    {
        #region Fields

        private static UndyingCandles _plugin;
        private Harmony _harmony;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
            _harmony = new Harmony(Name + "PATCH");
            _harmony.PatchAll();
        }

        private void Unload()
        {
            _harmony.UnpatchAll(Name + "PATCH");
            _plugin = null;
        }

        private object OnCandleUpdateInvokes(Candle candle)
        {
            if (candle == null)
                return null;

            return true;
        }

        #endregion Oxide Hooks

        #region Harmony Patches

        [HarmonyPatch(typeof(Candle), "UpdateInvokes")]
        public static class Candle_UpdateInvokes_Patch
        {
            public static bool Prefix(Candle __instance)
            {
                if (Interface.CallHook("OnCandleUpdateInvokes", __instance) != null)
                {
                    // Return a non-null value to block the original method, null to allow it.
                    return false;
                }

                return true;
            }
        }

        #endregion Harmony Patches
    }
}