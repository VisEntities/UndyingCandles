/*
 * Copyright (C) 2024 Game4Freak.io
 * This mod is provided under the Game4Freak EULA.
 * Full legal terms can be found at https://game4freak.io/eula/
 */

using HarmonyLib;
using Oxide.Core;
using Oxide.Core.Plugins;
using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("Undying Candles", "VisEntities", "1.1.0")]
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
            PermissionUtil.RegisterPermissions();
        }

        private void Unload()
        {
            _plugin = null;
        }

        private object OnCandleUpdateInvokes(Candle candle)
        {
            if (candle == null)
                return null;

            BasePlayer ownerPlayer = FindPlayerById(candle.OwnerID);
            if (ownerPlayer != null && PermissionUtil.HasPermission(ownerPlayer, PermissionUtil.USE))
                return true;

            return null;
        }

        #endregion Oxide Hooks

        #region Permissions

        private static class PermissionUtil
        {
            public const string USE = "undyingcandles.use";

            private static readonly List<string> _permissions = new List<string>
            {
                USE,
            };

            public static void RegisterPermissions()
            {
                foreach (var permission in _permissions)
                {
                    _plugin.permission.RegisterPermission(permission, _plugin);
                }
            }

            public static bool HasPermission(BasePlayer player, string permissionName)
            {
                return _plugin.permission.UserHasPermission(player.UserIDString, permissionName);
            }
        }

        #endregion Permissions

        #region Helper Functions

        public static BasePlayer FindPlayerById(ulong playerId)
        {
            return RelationshipManager.FindByID(playerId);
        }

        #endregion Helper Functions

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