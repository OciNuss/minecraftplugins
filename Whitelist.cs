using UnityEngine;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using ConVar;
using Oxide.Game.Rust.Cui;

namespace Oxide.Plugins
{
    [Info("Whitelist", "Jerry", "1.0.0")]
    [Description("simple whitelist plugin")]

    public class Whitelist : RustPlugin
    {
        private List<ulong> Friends = new List<ulong>
        {
            76561198209168347, //Jerry
            76561198875397422, //Sui
            76561199223718725, //slesh
            76561199367663428, //sheff
            76561199086732373, //kakaha
            76561199119078088, //sheff 2nd acc
            76561199442934152 //reywell
        };

        void OnPlayerConnected(BasePlayer player)
        {
            if (Friends.Contains(player.userID))
            {
                foreach (var onlinePlayer in BasePlayer.activePlayerList)
                {
                   onlinePlayer.ChatMessage(player.displayName + " connected");
                }
            }
            else
            {
                player.Kick("You are not whitelisted");
                foreach (var onlinePlayer in BasePlayer.activePlayerList)
                {
                    onlinePlayer.ChatMessage(player.displayName + " disconnected (not whitelisted, rip bozo)");
                }
            }
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (Friends.Contains(player.userID))
            {
                foreach (var onlinePlayer in BasePlayer.activePlayerList)
                {
                    onlinePlayer.ChatMessage(player.displayName + " disconnected");
                }
            }
        }
    }
}
