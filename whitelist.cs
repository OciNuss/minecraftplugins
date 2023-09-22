using UnityEngine;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using ConVar;
using Oxide.Game.Rust.Cui;

namespace Oxide.Plugins
{
    [Info("whitelist", "Jerry", "1.0.0")]
    [Description("simple whitelist plugin")]

    public class whitelist : RustPlugin
    {
        //create a list of strings
        private List<ulong> Friends = new List<ulong>
        {
            76561198209168347, //Jerry
            76561198083320431, //Paulotelli
            76561198186683594, //Bloody
            76561198242069450, //Niki
            76561198306452916, //Benji
            76561198153215070, //Ninsa
            76561198964103379, //Pfob
            76561199018313323 // acti
        };

        void OnPlayerConnected(BasePlayer player)
        {
            if (Friends.Contains(player.userID))
            {
                foreach (var onlinePlayer in BasePlayer.activePlayerList)
                {
                   player.ChatMessage(player.displayName + " connected");
                }
            }
            else
            {
                player.Kick("You are not whitelisted");
                foreach (var onlinePlayer in BasePlayer.activePlayerList)
                {
                    player.ChatMessage(player.displayName + " disconnected (not whitelisted)");
                }
            }
        }

        void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            foreach (var onlinePlayer in BasePlayer.activePlayerList)
            {
                player.ChatMessage(player.displayName + " disconnected");
            }
        }
    }
}
