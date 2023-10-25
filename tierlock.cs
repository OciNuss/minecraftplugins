using UnityEngine;
using Oxide.Core.Plugins;
using System;
using System.Collections.Generic;
using ConVar;
using Oxide.Game.Rust.Cui;

namespace Oxide.Plugins
{
    [Info("tierlock", "Jerry", "1.0.0")]
    [Description("locks tiers for a timeperiod")]

    public class tierlock : RustPlugin
    {
        #region main

        //Save wipe time
        public float wipeTime = UnityEngine.Time.time;
        public float lastUIUpdate = UnityEngine.Time.time;

        //Time each tier is the max for in hours
        //once tier2Time has elapsed, all tiers are unlocked
        public int tier0Time = 2;
        public int tier1Time = 6;
        public int tier2Time = 24;

        bool CanEquipItem(PlayerInventory inventory, Item item, int targetPos)
        {
            //check if item is weapon, if not always allow equip
            if (item.info.category != ItemCategory.Weapon) return true;

            //check weapon tier
            int itemTier = item.info.Blueprint.workbenchLevelRequired;
            if (itemTier > GetCurrentTier())
            {
                //tier is locked, deny equip
                return false;
            }

            //tier is unlocked, allow equip
            return true;
        }

        #endregion

        #region Helpers

        int GetCurrentTier()
        {
            if (wipeTime < tier0Time * 3600) return 0;
            else if (wipeTime < tier1Time * 3600) return 1;
            else if (wipeTime < tier2Time * 3600) return 2;
            else return 3;
        }

        string GetTierName(int tier)
        {
            switch (tier)
            {
                case 0:
                    return "Current tier: Primitive";
                case 1:
                    return "Current tier: Tier 1";
                case 2:
                    return "Current tier: Tier 2";
                default:
                    return "All tiers unlocked";
            }
        }

        string GetTimeLeft(int tier)
        {
            float currTime = UnityEngine.Time.time;
            switch (tier)
            {
                case 0:
                    return "Time left: " + (tier0Time * 3600 - currTime).ToString();
                case 1:
                    return "Time left: " + (currTime - tier1Time * 3600 - currTime).ToString();
                case 2:
                    return "Time left: " + (currTime - tier2Time * 3600 - currTime).ToString();
                default:
                    return "";
            }
        }

        #endregion

        #region UI

        void OnTick()
        {
            if (UnityEngine.Time.time - lastUIUpdate > 1)
            {
                lastUIUpdate = UnityEngine.Time.time;
                ReloadGlobalUI();
            }
        }

        void OnPlayerConnected(BasePlayer player)
        {
            DrawUI(player);
        }

        void DrawUI(BasePlayer player)
        {
            var container = new CuiElementContainer();
            container.Add(new CuiElement
            {
                Name = "Head",
                Components =
                {
                    new CuiTextComponent {
                        Text = GetTierName(GetCurrentTier()),
                        Font = "robotocondensed-bold.ttf",
                        FontSize = 20,
                        Align = TextAnchor.MiddleCenter,
                        Color = "1 1 1 1" },
                    new CuiOutlineComponent {
                        Color = "0 0 0 0.5",
                        Distance = "1 -1" },
                    new CuiRectTransformComponent {
                        AnchorMin = "0.95 0.95",
                        AnchorMax = "0.95 0.95",
                        OffsetMin = "-310.978 -15",
                        OffsetMax = "310.982 15"
                    }
                }
            });
            container.Add(new CuiElement
            {
                Name = "TimeLeft",
                Components =
                {
                    new CuiTextComponent {
                        Text = GetTierName(GetCurrentTier()),
                        Font = "robotocondensed-bold.ttf",
                        FontSize = 20,
                        Align = TextAnchor.MiddleCenter,
                        Color = "1 1 1 1" },
                    new CuiOutlineComponent {
                        Color = "0 0 0 0.5",
                        Distance = "1 -1" },
                    new CuiRectTransformComponent {
                        AnchorMin = "0.95 0.93",
                        AnchorMax = "0.95 0.93",
                        OffsetMin = "-310.978 -15",
                        OffsetMax = "310.982 15"
                    }
                }
            });
            CuiHelper.AddUi(player, container);
        }

        void ClearUI(BasePlayer player)
        {
            CuiHelper.DestroyUi(player, "Head");
            CuiHelper.DestroyUi(player, "TimeLeft");
        }

        void ReloadGlobalUI()
        {
            foreach (BasePlayer player in BasePlayer.activePlayerList)
            {
                ClearUI(player);
                DrawUI(player);
            }
        }

        #endregion
    }
}
