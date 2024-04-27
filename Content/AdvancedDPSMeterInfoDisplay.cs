using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using VoxMod.Common;
using System.Runtime.Intrinsics.X86;
using Terraria.Localization;

namespace VoxMod.Content
{
	public class AdvancedDPSMeterInfoDisplay : InfoDisplay
    {

        // By default, the vanilla circular outline texture will be used. 
        // This info display has a square icon instead of a circular one, so we need to use a custom outline texture instead of the vanilla outline texture.
        // You will only need to use a custom hover texture if your info display icon doesn't perfectly match the shape that vanilla info displays use
        public override string HoverTexture => Texture + "_Hover";


        // This dictates whether or not this info display should be active
        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<AdvancedDPSMeterInfoDisplayPlayer>().showAdvDPSMeter;
        }

        // Here we can change the value that will be displayed in the game
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            // Get total damage on player
            int dps = (int)Main.LocalPlayer.GetModPlayer<AdvancedDPSMeterInfoDisplayPlayer>().GetDPS();
            int maximumDPS = (int) Main.LocalPlayer.GetModPlayer<AdvancedDPSMeterInfoDisplayPlayer>().GetMaximumDPS();

            // Set display color
            displayColor = new Color(230, 230, 230);

            // Return string which is shown on screen
            return $"{dps} ({maximumDPS}) average DPS ";
        }
    }
}
