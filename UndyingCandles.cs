/*
 * Copyright (C) 2024 Game4Freak.io
 * This mod is provided under the Game4Freak EULA.
 * Full legal terms can be found at https://game4freak.io/eula/
 */

using HarmonyLib;
using Oxide.Core;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Undying Candles", "VisEntities", "1.0.1")]
    [Description("Candles never burn out, providing constant light.")]
    public class UndyingCandles : RustPlugin
    {
        #region Fields

        private static UndyingCandles _plugin;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
        }

        private void Unload()
        {
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

        [AutoPatch]
        [HarmonyPatch(typeof(Candle), "UpdateInvokes")]
        public static class Candle_UpdateInvokes_Patch
        {
            public static bool Prefix(Candle __instance)
            {
                if (Interface.CallHook("OnCandleUpdateInvokes", __instance) != null)
                    return false;

                return true;
            }
        }

        #endregion Harmony Patches
    }
}