
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using VoxMod.Common;

// This file contains fake ModConfig class that showcase creating config section
// by using fields with various data types.

// Because this config was designed to show off various UI capabilities,
// this config have no effect on the mod and provides purely teaching example.
namespace ExampleMod.Common.Configs.ModConfigSettings
{
    [BackgroundColor(144, 252, 249)]
    public class ModConfigSettings : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("$Mods.VoxMod.Configs.DPSMeter")]

        [DefaultValue(3)]
        [Range(.5f, 5f)]
        [Increment(.25f)]
        [DrawTicks]
        public float EWAWindowInSeconds;

        [DefaultValue(1.25f)]
        [Range(0f, 5f)]
        [Increment(.25f)]
        [DrawTicks]
        public float EWASmoothing;

        [DefaultValue(true)]
        public bool EnableFastOpen;

        [DefaultValue(1.5f)]
        [Range(0.25f, 5f)]
        [Increment(.25f)]
        [DrawTicks]
        public float FastOpenReEnableTimeoutInSeconds;


        public override void OnChanged()
        {
            base.OnChanged();
            AdvancedDPSMeterInfoDisplayPlayer dpsDisplayPlayer = null;
            Main.LocalPlayer.TryGetModPlayer(out dpsDisplayPlayer);
            if (dpsDisplayPlayer != null) dpsDisplayPlayer.LoadFromModConfig();
        }
    }
}
