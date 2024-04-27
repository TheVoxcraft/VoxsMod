using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using log4net.Repository.Hierarchy;
using Terraria.ModLoader.Config;
using ExampleMod.Common.Configs.ModConfigSettings;

namespace VoxMod.Common
{
	public class AdvancedDPSMeterInfoDisplayPlayer : ModPlayer
    {
        // Flag checking when information display should be activated
        public bool showAdvDPSMeter;

        private const int TICKS_PER_SECOND = 60;

        private float EWADamagePerTick = 0;
        private float MaximumDPS = 0;
        private int damageAccumulator = 0;

        public float EWAWindowInSeconds = 3;
        public float EWASmoothing = 1.25f;
        public bool FastOpenEnabled = true;
        public int FOTickTimeout = (int) 1.5 * TICKS_PER_SECOND;

        private bool FOdoFastOpen = true;
        private int FOTicksSinceDamage = 0;

        // Make sure to use the right Reset hook. This one is unique, as it will still be
        // called when the game is paused; this allows for info accessories to keep updating properly.
        public override void ResetInfoAccessories()
        {
            showAdvDPSMeter = false;
        }

        public void LoadFromModConfig()
        {
            ModConfigSettings config = ModContent.GetInstance<ModConfigSettings>();
            if (config == null) return;
            EWAWindowInSeconds = config.EWAWindowInSeconds;
            EWASmoothing = config.EWASmoothing;
            FastOpenEnabled = config.EnableFastOpen;
            FOTickTimeout = (int)config.FastOpenReEnableTimeoutInSeconds * TICKS_PER_SECOND;
        }

        public override void OnEnterWorld()
        {
            base.OnEnterWorld();
            LoadFromModConfig();
        }

        // If we have another nearby player on our team, we want to get their info accessories working on us,
        // just like in vanilla. This is what this hook is for.
        public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer)
        {
            if (otherPlayer.GetModPlayer<AdvancedDPSMeterInfoDisplayPlayer>().showAdvDPSMeter)
            {
                showAdvDPSMeter = true;
            }
        }
        private float CalculateExponentialMovingAverage(int damageWithinTick)
        {
            float alpha = EWASmoothing / (1 + (EWAWindowInSeconds * TICKS_PER_SECOND));
            return (damageWithinTick * alpha) + (EWADamagePerTick * (1 - alpha));
        }

        public override void PostUpdate()
        {
            base.PostUpdate();
            EWADamagePerTick = CalculateExponentialMovingAverage(damageAccumulator);
            damageAccumulator = 0;

            if (EWADamagePerTick >= MaximumDPS) MaximumDPS = EWADamagePerTick;

            if (damageAccumulator == 0)
            {
                FOTicksSinceDamage++;
            }
            if (!FOdoFastOpen && FOTicksSinceDamage >= FOTickTimeout) FOdoFastOpen = true;
        }

        public float GetDPS()
        {
            return EWADamagePerTick * TICKS_PER_SECOND;
        }

        public float GetMaximumDPS()
        {
            return MaximumDPS * TICKS_PER_SECOND;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            damageAccumulator += damageDone;

            FOTicksSinceDamage = 0;
            if (FastOpenEnabled && FOdoFastOpen)
            {
                EWADamagePerTick = (damageDone * .8f) / TICKS_PER_SECOND;
                MaximumDPS = 0; // Good time to reset the maximum
                FOdoFastOpen = false;
            }
        }
    }
}
